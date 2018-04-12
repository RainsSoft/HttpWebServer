using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HttpServer.Logging;
using HttpServer.Mvc;
using HttpServer.Mvc.Controllers;
using HttpServer.Mvc.Views;
using Spark;

namespace HttpServer.ViewEngine.Spark
{
    /// <summary>
    /// Implementation of spark view engine.
    /// </summary>
    /// <remarks>
    /// Compared to Asp.NET MVC version, we use strongly typed views. You can
    /// access all view data directly in your views by using ${yourPropertyName}. No need
    /// to use the <![CDATA[<viewdata ..../>]]> tag.
    /// </remarks>
    /// 
    public class SparkEngine : IViewEngine
    {
        private readonly Type _viewBaseType;
        private readonly InitializeViewHandler _handler;
        private ILogger _logger = LogFactory.CreateLogger(typeof (SparkEngine));
        [ThreadStatic] private static SparkEngine _currentEngine;
        private const string DEFAULT_LAYOUT = "/shared/application.spark";

        /// <summary>
        /// string = view Uri path.
        /// </summary>
        private readonly Dictionary<string, Mapping> _ajaxMappings = new Dictionary<string, Mapping>();

        private readonly SparkViewEngine _engine;

        /// <summary>
        /// string = view Uri path.
        /// </summary>
        private readonly Dictionary<string, Mapping> _mappings = new Dictionary<string, Mapping>();


        /// <summary>
        /// Initializes a new instance of the <see cref="SparkEngine"/> class.
        /// </summary>
        public SparkEngine()
        {
            _currentEngine = this;
            SparkSettings settings = new SparkSettings();
            settings.AddNamespace("System.Collections.Generic");
            _engine = new SparkViewEngine(settings)
                          {
                              ViewFolder = new MyViewFolder(),
                              DefaultPageBaseType = typeof (SparkView).FullName
                          };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkEngine"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><c>viewBaseType</c> or <c>handler</c> is <c>null</c>.</exception>
        public SparkEngine(Type viewBaseType, InitializeViewHandler handler)
        {
            if (viewBaseType == null) throw new ArgumentNullException("viewBaseType");
            if (handler == null) throw new ArgumentNullException("handler");
            _viewBaseType = viewBaseType;
            _handler = handler;
            _currentEngine = this;

            SparkSettings settings = new SparkSettings();
            settings.AddNamespace("System.Collections.Generic");
            settings.AddNamespace("HttpServer.Mvc.Views");
            settings.AddNamespace("HttpServer.ViewEngine.Spark");
            settings.AddAssembly(GetType().Assembly);
            settings.AddAssembly(typeof (Server).Assembly);
            _engine = new SparkViewEngine(settings)
            {
                ViewFolder = new MyViewFolder(),
                DefaultPageBaseType = viewBaseType.FullName
            };
        }



        /// <summary>
        /// Currently running view engine.
        /// </summary>
        internal SparkEngine Current
        {
            get { return _currentEngine; }
        }

        /// <summary>
        /// Gets a list of file extensions that this engine use.
        /// </summary>
        public IEnumerable<string> FileExtensions
        {
            get { return new[] {"spark"}; }
        }

        /// <summary>
        /// Used to get correct names for generics.
        /// </summary>
        /// <param name="type">Type to generate a strig name for.</param>
        /// <param name="useFullName">true if FullName should be used (including namespace in typename)</param>
        /// <returns>Type as a code string</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string typeName = typeof(List<string>).Name; // will become: List`1
        /// typeName = Compiler.GetTypeName(typeof(List<string>)); // will become: System.Collections.Generic.List<string>
        /// ]]>
        /// </code>
        /// </example>
        public static string GetTypeName(Type type, bool useFullName)
        {
			string typeName = useFullName ? type.FullName : type.Name;
			if (type.IsNested)
				typeName = typeName.Replace('+', '.');

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(typeName.Substring(0, typeName.IndexOf('`')));
                sb.Append("<");
                bool first = true;
                foreach (Type genericArgumentType in type.GetGenericArguments())
                {
                    if (!first)
                        sb.Append(", ");
                    first = false;
                    sb.Append(GetTypeName(genericArgumentType, useFullName));
                }
                sb.Append(">");
                return sb.ToString();
            }

            return typeName;
        }

        private string SelectLayoutName(string specifiedName)
        {
            if (string.IsNullOrEmpty(specifiedName))
                return DEFAULT_LAYOUT;

            if (specifiedName.Contains("."))
            {
                if (MvcServer.CurrentMvc.ViewProvider.Exists(specifiedName))
                {
                    _logger.Debug("Layout specified as file, no path is added: " + specifiedName);
                    return specifiedName;
                }

                _logger.Info("Layout specified as a file (uri), but the file was not found: " + specifiedName);
            }

            if (!string.IsNullOrEmpty(specifiedName))
            {
                string layoutUri = "/shared/" + specifiedName + ".spark";
                if (MvcServer.CurrentMvc.ViewProvider.Exists(layoutUri))
                {
                    _logger.Debug("Layout specified as view, adding path: /shared/" + specifiedName + ".spark");
                    return layoutUri;
                }

                _logger.Info("Layout was specified with a name, but it was not found.");
            }

            _logger.Debug("No layout is specified, using default: " + DEFAULT_LAYOUT);
            return DEFAULT_LAYOUT;
        }

        /// <summary>
        /// Create view
        /// </summary>
        /// <param name="context">Context used creating view</param>
        /// <param name="viewUri">Path to view being created.</param>
        /// <param name="data">Data used.</param>
        /// <returns>Created view.</returns>
        /// <exception cref="ViewNotFoundException">Failed to find spark view.</exception>
        private SparkView GenerateView(IControllerContext context, string viewUri, IViewData data)
        {
            var descriptor = new SparkViewDescriptor();
            descriptor.AddTemplate(viewUri);

            // Let's not include layouts for ajax requests.
            if (!context.RequestContext.Request.IsAjax)
            {
                string layoutUri = SelectLayoutName(context.LayoutName);
                descriptor.Templates.Add(layoutUri);
            }

            if (!MvcServer.CurrentMvc.ViewProvider.Exists(viewUri))
            {
                _logger.Info("Failed to find " + viewUri);
                throw new ViewNotFoundException(viewUri, "Failed to find spark view '" + viewUri + "'.");
            }


            /* Disabled since types can be null.
             * 
            // since we use strongly typed accessors, we need to loop through view data.
            // and add direct accessors.
            foreach (var viewData in data)
            {
                string typeName = GetTypeName(viewData.Value.GetType(), true);
                descriptor.AddAccessor(typeName + " " + viewData.Key,
                                       "(" + typeName + ")ViewData[\"" + viewData.Key + "\"]");
            }
            */

            ISparkViewEntry entry;
            try
            {
                entry = _engine.CreateEntry(descriptor);
            }
            catch(Exception err)
            {
                _logger.Warning("Failed to compile view.", err);
                throw new InternalServerException("Failed to compile view '" + viewUri + "'.", err);
            }

            // only lock when adding, to avoid duplicates.
            if (context.RequestContext.Request.IsAjax)
            {
                lock (_ajaxMappings)
                {
                    Mapping mapping;
                    if (!_ajaxMappings.TryGetValue(viewUri, out mapping))
                    {
                        mapping = new Mapping(data.GetHashCode(), entry);
                        _ajaxMappings.Add(viewUri, mapping);
                    }
                    else
                        mapping.Add(data.GetHashCode(), entry);
                }
            }
            else
            {
                lock (_mappings)
                {
                    Mapping mapping;
                    if (!_mappings.TryGetValue(viewUri, out mapping))
                    {
                        mapping = new Mapping(data.GetHashCode(), entry);
                        _mappings.Add(viewUri, mapping);
                    }
                    else
                        mapping.Add(data.GetHashCode(), entry);
                }
            }

            return (SparkView) entry.CreateInstance();
        }

        private Mapping GetMapping(bool isAjax, string uri)
        {
            Mapping mapping;
            if (isAjax)
                return _ajaxMappings.TryGetValue(uri, out mapping) ? mapping : null;
            return _mappings.TryGetValue(uri, out mapping) ? mapping : null;
        }

        #region IViewEngine Members

        /// <summary>
        /// Create a view.
        /// </summary>
        /// <param name="context">Request and controller information.</param>
        /// <param name="viewData">Information that is used in the view</param>
        /// <param name="writer">Write the view using this writer.</param>
        public void Render(IControllerContext context, IViewData viewData, TextWriter writer)
        {
            _currentEngine = this;

            string viewUri = context.ViewPath + ".spark";

            try
            {
                // try to find our view, or create it if it do not exist.
                SparkView view = GenerateView(context, viewUri, viewData);

                // attach view data and render it.
                view.ViewData = viewData;
                view.Uri = context.RequestContext.Request.Uri;
                view.ControllerName = context.ControllerName;
                view.ControllerUri = context.ControllerUri;
                view.Title = context.Title;
                view.ActionName = context.ActionName;

                // Using custom view, let the implementor have a chance to init it.
                if (_handler != null)
                    _handler(view, context);

                view.RenderView(writer);

                // release it.
                _engine.ReleaseInstance(view);

                writer.Flush();
            }
            catch(Exception err)
            {
                _logger.Error("Failed to render " + viewUri + " using viewdata " + ViewDataToString(viewData));
                throw new InvalidOperationException("Failed to render " + viewUri, err);
            }
        }

        public string ViewDataToString(IViewData viewData)
        {
            var str = string.Empty;
            foreach (var data in viewData)
                str += string.Format("{0} = {1}, ", data.Key, data.Value ?? "NULL!");

            return str.Length > 0 ? str.Remove(str.Length - 2, 2) : str;
        }

        /// <summary>
        /// Gets file extension used by engine.
        /// </summary>
        /// <remarks>
        /// File extension should not contain a dot.
        /// </remarks>
        public string FileExtension
        {
            get { return "spark"; }
        }

        #endregion

        #region Nested type: Mapping

        /// <summary>
        /// Maps view data to a strongly typed view.
        /// </summary>
        /// <remarks>
        /// <para>We are always creating strongly typed views, therefore
        /// we need to keep track of each generated view. ViewData.GetHashCode()
        /// is used to separate all views.</para>
        /// </remarks>
        private class Mapping
        {
            private readonly Dictionary<int, ISparkViewEntry> _entries = new Dictionary<int, ISparkViewEntry>();

            public Mapping(int hashCode, ISparkViewEntry entry)
            {
                Add(hashCode, entry);
            }

            public void Add(int hashCode, ISparkViewEntry entry)
            {
                _entries[hashCode] = entry;
            }

            public SparkView Create(int hashCode)
            {
                ISparkViewEntry entry;
                return _entries.TryGetValue(hashCode, out entry) ? (SparkView) entry.CreateInstance() : null;
            }
        }

        #endregion
    }

    /// <summary>
    /// Invoked before a view is rendered when custom views are used.
    /// </summary>
    /// <param name="view">View to invoke</param>
    /// <param name="context">Current controller context.</param>
    public delegate void InitializeViewHandler(SparkView view, IControllerContext context);
}