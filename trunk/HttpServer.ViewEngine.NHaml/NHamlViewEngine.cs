using System;
using System.Collections.Generic;
using System.IO;
using HttpServer.Mvc;
using HttpServer.Mvc.Controllers;
using HttpServer.Mvc.Views;
using HttpServer.Resources;
using NHaml;
using NHaml.TemplateResolution;

namespace HttpServer.ViewEngine.NHaml
{
    /// <summary>
    /// Implementation of nhaml view engine in our WebServer..
    /// </summary>
    public class NHamlViewEngine : IViewEngine, ITemplateContentProvider
    {
        private readonly TemplateEngine _templateEngine = new TemplateEngine();

        /// <summary>
        /// string = "layoutName|[uri to template]"
        /// </summary>
        private Dictionary<string, NHamlView> _templates = new Dictionary<string, NHamlView>();

        public NHamlViewEngine()
        {
            _templateEngine.Options.AddReferences(typeof (MvcServer));
            _templateEngine.Options.AddReferences(typeof (Server));
            _templateEngine.Options.TemplateBaseType = typeof (NHamlView);
            _templateEngine.Options.TemplateContentProvider = this;
        }

        private string GetOurPath(string path)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;
            return path.Replace('\\', '/').ToLower();
        }

        #region ITemplateContentProvider Members

        public IViewSource GetViewSource(string templateName)
        {
            templateName = GetOurPath(templateName);
            Resource resource = MvcServer.CurrentMvc.ViewProvider.Get(templateName);
            if (resource == null)
                throw new ViewNotFoundException(templateName, "Failed to find view.");

            return new ViewSource(resource, templateName);
        }

        public IViewSource GetViewSource(string templatePath, IList<IViewSource> parentViewSourceList)
        {
            return GetViewSource(templatePath);
        }

        public void AddPathSource(string pathSource)
        {
        }

        public IList<string> PathSources
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region IViewEngine Members

        /// <summary>
        /// Create a view.
        /// </summary>
        /// <param name="context">Context to render</param>
        public void Render(IControllerContext context, IViewData viewData, TextWriter writer)
        {
            string layoutName;
            if (context.LayoutName != null)
                layoutName = context.LayoutName;
            else
            {
                var controllerName = context.ControllerUri.TrimEnd('/');
                int pos = controllerName.LastIndexOf('/');
                layoutName = context.ControllerUri;
                layoutName += pos == -1
                                  ? controllerName + ".haml"
                                  : controllerName.Substring(pos + 1) + ".haml";

                if (!MvcServer.CurrentMvc.ViewProvider.Exists(layoutName))
                    layoutName = "Shared/Application.haml";
            }

            string viewPath = context.ViewPath + ".haml";


            CompiledTemplate template = _templateEngine.Compile(new List<string> {layoutName, viewPath},
                                                                typeof (NHamlView));

            var instance = (NHamlView)template.CreateInstance();
            instance.ViewData = viewData;
            instance.Render(writer);
        }

        /// <summary>
        /// Gets file extension used by engine.
        /// </summary>
        /// <remarks>
        /// File extension should not contain a dot.
        /// </remarks>
        public string FileExtension
        {
            get { return "haml"; }
        }

        #endregion

    }
}