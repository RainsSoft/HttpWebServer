using System;

namespace HttpServer.Mvc.Controllers
{
    /// <summary>
    /// Context used in controllers.
    /// </summary>
    /// <remarks>A context is used in the controller instead of having to set a lot
    /// of controller properties each time a new request is about to be processed.
    /// In this way it's up to the controller creator do decide with which parameters
    /// that should be exposed to the developer.</remarks>
    public class ControllerContext : IControllerContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerContext"/> class.
        /// </summary>
        /// <param name="context">Request context.</param>
        public ControllerContext(RequestContext context)
        {
            RequestContext = context;
            Uri = context.Request.Uri;
            UriSegments = Uri.AbsolutePath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
        }

        #region IControllerContext Members

        /// <summary>
        /// Gets or sets executing controller
        /// </summary>
        public object Controller { get; set; }

        /// <summary>
        /// Gets or sets controller uri.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// A controller doesn't necessarily have to use the "/controllerName/" uri,
        /// but can exist in sub folders like "/area/area2/controllerName". This feature
        /// is controlled by the <see cref="ControllerUriAttribute"/>.
        /// </remarks>
        public string ControllerUri { get; set; }

        /// <summary>
        /// Gets or sets path and name of view to render, excluding file extension.
        /// </summary>
        public string ViewPath { get; set; }

        /// <summary>
        /// Gets or sets action name.
        /// </summary>
        /// <remarks>
        /// Will be filled in by controller director if empty (i.e. default action).
        /// </remarks>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets layout to render.
        /// </summary>
        /// <value>
        /// <c>null</c> if default layout should be used.
        /// </value>
        /// <remarks>
        /// Only specified if sets it.
        /// </remarks>
        public string LayoutName { get; set; }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public RequestContext RequestContext { get; private set; }

        /// <summary>
        /// Gets or sets requested URI.
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets path split up on slash.
        /// </summary>
        public string[] UriSegments { get; private set; }

        /// <summary>
        /// Gets or sets document title.
        /// </summary>
        public string Title
        {
            get; set;
        }

        /// <summary>
        /// Gets name of controller
        /// </summary>
        /// <remarks>
        /// Might include slashes if the controller is a nested controller.
        /// </remarks>
        public string ControllerName
        {
            get; set;
        }

        /// <summary>
        /// Get a parameter.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Parameter value if found; otherwise <c>null</c>.</returns>
        /// <remarks>
        /// Wrapper around query string and form values.
        /// </remarks>
        public string this[string name]
        {
            get { return RequestContext.Request.Parameters[name]; }
        }

        #endregion
    }
}