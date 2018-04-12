using System.IO;

namespace HttpServer.Mvc.Views.TinyEngine
{
    /// <summary>
    /// View in <see cref="TinyViewEngine"/>.
    /// </summary>
    public abstract class TinyView
    {
        /// <summary>
        /// Render view into the supplied text writer.
        /// </summary>
        /// <param name="writer">Text writer to render in.</param>
        /// <param name="viewData">Supplied view data</param>
        public abstract void Render(TextWriter writer, IViewData viewData);
    }
}