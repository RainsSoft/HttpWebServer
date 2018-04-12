using HttpServer.Mvc.Controllers;

namespace HttpServer.Mvc.Views
{
    /// <summary>
    /// Information needed by view engines to be able to render correctly.
    /// </summary>
    public class ViewContext : IViewContext
    {
        #region IViewContext Members

        /// <summary>
        /// Gets executing controller.
        /// </summary>
        public Controller Controller { get; set; }

        /// <summary>
        /// Gets view to render.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets layout to render view in.
        /// </summary>
        /// <remarks>
        /// Empty if a Layout should not be used. Controllers will automatically
        /// remove layout name if Ajax requests are used.
        /// </remarks>
        public string LayoutName { get; set; }

        #endregion
    }
}