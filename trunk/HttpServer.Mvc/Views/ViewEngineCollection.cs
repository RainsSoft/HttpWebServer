using System;
using System.Collections.Generic;
using System.IO;
using HttpServer.Mvc.Controllers;

namespace HttpServer.Mvc.Views
{
    /// <summary>
    /// Collection of view engines.
    /// </summary>
    public class ViewEngineCollection
    {
        private readonly Dictionary<string, IViewEngine> _viewEngines = new Dictionary<string, IViewEngine>();

        /// <summary>
        /// Add a engine.
        /// </summary>
        /// <param name="engine">Engine to add</param>
        /// <exception cref="ArgumentNullException"><c>engine</c> is null.</exception>
        public void Add(IViewEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            _viewEngines.Add(engine.FileExtension, engine);
        }

        /// <summary>
        /// Render a view.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="context"></param>
        /// <param name="viewData"></param>
        public void Render(TextWriter writer, ControllerContext context, IViewData viewData)
        {
            foreach (IViewEngine engine in _viewEngines.Values)
            {
                engine.Render(context, viewData, writer);
                writer.Flush();
            }
        }
    }
}