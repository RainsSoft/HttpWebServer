using System;
using System.Collections.Generic;

namespace HttpServer.Mvc.Controllers
{
    /// <summary>
    /// Takes care of all controller processing.
    /// </summary>
    /// <remarks>
    /// A controller director is responsible of invoking the correct method in a controller
    /// and return the result. In this way we can have numerous of different controller
    /// implementations in the system. Is up to each controller director type to create
    /// and maintain controllers.
    /// </remarks>
    public interface IControllerDirector
    {
        /// <summary>
        /// Get all routings
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetRoutes();

        /// <summary>
        /// Process controller request.
        /// </summary>
        /// <param name="context">Controller context</param>
        /// <param name="controller">Controller used to process request</param>
        /// <returns>Action result.</returns>
        /// <exception cref="NotFoundException"><c>NotFoundException</c>.</exception>
        /// <exception cref="InvalidOperationException">Specified URI is not for this controller.</exception>
        /// <remarks>
        /// Controller is returned as a parameter to let us be able to use it as a variable
        /// in the view. It's recommended that you call <see cref="Enqueue"/> when rendering is complete (or if an exception
        /// is thrown) to let the controller get reused.
        /// </remarks>
        object Process(IControllerContext context, out Controller controller);

        /// <summary>
        /// Enqueue a used controller.
        /// </summary>
        /// <param name="controller">Controller to enqueue</param>
        void Enqueue(Controller controller);

        /// <summary>
        /// Raised before a controller action is invoked.
        /// </summary>
        /// <remarks>Use it to invoke any controller initializations in your application.</remarks>
        event EventHandler<ControllerEventArgs> InvokingAction;
    }
}