using HttpServer.Mvc.Controllers;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Information needed by view engines to be able to render correctly.
    /// </summary>
    public interface IViewContext
    {
        /// <summary>
        /// Gets executing controller.
        /// </summary>
        Controller Controller { get; }

        /// <summary>
        /// Gets layout to render view in.
        /// </summary>
        /// <remarks>
        /// Empty if a Layout should not be used. Controllers will automatically
        /// remove layout name if Ajax requests are used.
        /// </remarks>
        string LayoutName { get; }

        /// <summary>
        /// Gets view to render.
        /// </summary>
        string ViewName { get; }
    }
}