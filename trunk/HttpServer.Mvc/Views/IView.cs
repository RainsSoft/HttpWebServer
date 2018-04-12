using System.IO;

namespace HttpServer.Mvc.Views
{
    /// <summary>
    /// View to render.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Write view information
        /// </summary>
        /// <param name="writer">Writer that view is written to</param>
        /// <param name="viewData">View data</param>
        void Render(TextWriter writer, IViewData viewData);
    }
}