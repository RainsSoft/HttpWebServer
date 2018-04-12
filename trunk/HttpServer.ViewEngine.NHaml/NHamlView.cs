using HttpServer.Mvc;
using NHaml;

namespace HttpServer.ViewEngine.NHaml
{
    /// <summary>
    /// Base class for NHaml views
    /// </summary>
    public class NHamlView : Template
    {
        /// <summary>
        /// Gets view data
        /// </summary>
        public IViewData ViewData { get; internal set; }
    }
}