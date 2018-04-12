using System;
using HttpServer.Mvc.Controllers;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Controller is about to be invoked, or have been invoked.
    /// </summary>
    public class ControllerEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerEventArgs"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="requestContext">The request context.</param>
        public ControllerEventArgs(Controller controller, string actionName, RequestContext requestContext)
        {
            Controller = controller;
            ActionName = actionName;
            RequestContext = requestContext;
        }

        /// <summary>
        /// Gets requested action.
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// Gets requested controller.
        /// </summary>
        public Controller Controller { get; private set; }

        /// <summary>
        /// Gets request context.
        /// </summary>
        public RequestContext RequestContext { get; private set; }
    }
}