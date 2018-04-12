using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HttpServer.Logging;
using HttpServer.Mvc.Controllers;
using HttpServer.Mvc.Views.TinyEngine;
using HttpServer.Resources;

namespace HttpServer.Mvc.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class TinyViewEngine : IViewEngine
    {
        private const string ClassContents =
            @"
namespace HttpServer.Mvc.Views.TinyEngine
{
	public class MyView : TinyView
	{
		public override void Render(TextWriter xyrxor, IViewData viewData)
		{
			{viewDataTypes}
			{body}
		}
	}
}";

        private readonly Dictionary<string, HashedViews> _cachedViews = new Dictionary<string, HashedViews>();
        private readonly ILogger _logger = LogFactory.CreateLogger(typeof (TinyViewEngine));
        private readonly ResourceProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyViewEngine"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public TinyViewEngine(ResourceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Gets a list of file extensions that this engine use.
        /// </summary>
        public IEnumerable<string> FileExtensions
        {
            get { return new[] {"tiny"}; }
        }

        private void Add(string path, int hashCode, TinyView view)
        {
            HashedViews views;
            lock (_cachedViews)
            {
                if (!_cachedViews.TryGetValue(path, out views))
                {
                    views = new HashedViews();
                    _cachedViews.Add(path, views);
                }
            }

            views.Add(hashCode, view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private TinyView CreateView(IControllerContext context, IViewData data)
        {
            string templateName = context.ViewPath + ".tiny";
            Resource resource = _provider.Get(templateName);
            if (resource == null)
            {
                _logger.Warning("Failed to load " + templateName);
                return null;
            }

            // Add all namespaces and assemblies needed.
            var compiler = new Compiler();
            compiler.Add(typeof (IView));
            compiler.Add(typeof (string));
            compiler.Add(typeof (TextWriter));
            foreach (var parameter in data)
                compiler.Add(parameter.Value.GetType());

            using (resource.Stream)
            {
                var buffer = new byte[resource.Stream.Length];
                resource.Stream.Read(buffer, 0, buffer.Length);

                TextReader reader = new StreamReader(resource.Stream);
                var sb = new StringBuilder();

                // parse
                Parse(reader, sb);

                // and add it in the type.
                string code = ClassContents;
                code = code.Replace("{body}", sb.ToString());

                // add view data types.
                string types = string.Empty;
                foreach (var parameter in data)
                {
                    types = Compiler.GetTypeName(parameter.Value.GetType()) + " " + parameter.Key + " = (" +
                            Compiler.GetTypeName(parameter.Value.GetType()) + ")viewData[\"" + parameter.Key + "\"];";
                }
                code = code.Replace("{viewDataTypes}", types);

                try
                {
                    compiler.Compile(code);
                }
                catch (Exception err)
                {
                    _logger.Error("Failed to compile template " + templateName + ", reason: " + err);
                }
                return compiler.CreateInstance<TinyView>();
            }
        }

        private TinyView GetView(string path, int hashCode)
        {
            HashedViews views;
            lock (_cachedViews)
            {
                if (!_cachedViews.TryGetValue(path, out views))
                    return null;
            }

            return views.Get(hashCode);
        }

        /// <summary>
        /// Parse a file and convert into to our own template object code.
        /// </summary>
        /// <param name="reader">A text reader containing our template</param>
        /// <param name="sb">String builder that the code is appended to</param>
        public void Parse(TextReader reader, StringBuilder sb)
        {
            bool inCode = false;
            bool isOutput = false;

            sb.Append("xyrxor.Write(@\"");
            string line = reader.ReadLine();
            while (line != null)
            {
                for (int i = 0; i < line.Length; ++i)
                {
                    char ch = line[i];
                    char nextCh = i < line.Length - 1 ? line[i + 1] : char.MinValue;


                    if (ch == '"')
                    {
                        sb.Append(ch);
                        if (!inCode)
                            sb.Append(ch);
                    }
                    else if (ch == '<' && nextCh == '%')
                    {
                        char thirdCh = i < line.Length - 2 ? line[i + 2] : char.MinValue;
                        ++i;

                        sb.Append("\");");
                        if (thirdCh == '=')
                        {
                            ++i;
                            isOutput = true;
                            sb.Append("xyrxor.Write(");
                        }

                        inCode = true;
                    }
                    else if (ch == '%' && nextCh == '>')
                    {
                        ++i;
                        if (isOutput)
                            sb.Append(");");

                        sb.Append("xyrxor.Write(@\"");
                        inCode = false;
                        isOutput = false;
                    }
                    else
                        sb.Append(ch);
                }

                sb.AppendLine();
                line = reader.ReadLine();
            } //while

            // to avoid compile errors.
            if (inCode && isOutput)
                sb.Append(");");
            else
                sb.Append("\");");
        }

        /// <summary>
        /// Render a layout
        /// </summary>
        /// <param name="writer">Writer to render to.</param>
        /// <param name="context">Controller context.</param>
        /// <param name="stream">Stream containing view data.</param>
        /// <param name="layoutName">Preferred layout, file extension is excluded.</param>
        public void RenderLayout(TextWriter writer, IControllerContext context, MemoryStream stream, string layoutName)
        {
            throw new NotImplementedException();
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
            string path = context.ViewPath + ".tiny";
            _logger.Trace("Rendering '" + path + "'");

            TinyView view = GetView(path, viewData.GetHashCode());
            if (view == null)
            {
                view = CreateView(context, viewData);
                Add(path, viewData.GetHashCode(), view);
            }

            if (view == null)
                return; //should throw an exception.

            try
            {
                view.Render(writer, viewData);
            }
            catch (Exception err)
            {
                _logger.Error("Failed to call view '" + path + "', reason: " + err);
                throw;
            }
        }

        /// <summary>
        /// Gets file extension used by engine.
        /// </summary>
        public string FileExtension
        {
            get { return "tiny"; }
        }

        #endregion

        #region Nested type: HashedViews

        private class HashedViews
        {
            private readonly Dictionary<int, TinyView> _hashedViews = new Dictionary<int, TinyView>();

            public void Add(int hashCode, TinyView hashedView)
            {
                lock (_hashedViews)
                    _hashedViews[hashCode] = hashedView;
            }

            public TinyView Get(int hashCode)
            {
                TinyView view;
                lock (_hashedViews)
                    return _hashedViews.TryGetValue(hashCode, out view) ? view : null;
            }
        }

        #endregion
    }
}