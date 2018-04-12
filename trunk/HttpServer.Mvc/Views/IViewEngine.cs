using System.IO;
using HttpServer.Mvc.Controllers;

namespace HttpServer.Mvc.Views
{
    /// <summary>
    /// View engine
    /// </summary>
    public interface IViewEngine
    {
        /// <summary>
        /// Gets file extension used by engine.
        /// </summary>
        /// <remarks>
        /// File extension should not contain a dot.
        /// </remarks>
        string FileExtension { get; }

        /// <summary>
        /// Create a view.
        /// </summary>
        /// <param name="context">Request and controller information.</param>
        /// <param name="viewData">Information that is used in the view</param>
        /// <param name="writer">Write the view using this writer.</param>
        void Render(IControllerContext context, IViewData viewData, TextWriter writer);
    }
}