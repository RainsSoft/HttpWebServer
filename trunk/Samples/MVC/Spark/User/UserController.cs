using HttpServer;
using HttpServer.Helpers;
using HttpServer.Mvc;
using HttpServer.Mvc.Controllers;

namespace Spark.User
{
    public class UserController : Controller, IController
    {
        /// <summary>
        /// Creates a new controller that is a clone of the current one.
        /// </summary>
        /// <returns>A new controller.</returns>
        public override object Clone()
        {
            return this;
        }

        /// <summary>
        /// Default controller method.
        /// </summary>
        /// <returns></returns>
        public IViewData Index()
        {
            // Parameters is a array merged with QueryString and Form variables.
            string userId = Parameters["userid"];

            ViewData["whom"] = "World";
            return Render();
        }

        /// <summary>
        /// Same method ("/user/index/"), but this one is only called
        /// from POST requests.
        /// </summary>
        /// <returns></returns>
        public IActionResult IndexPost()
        {
            // This is how easy a user is filled with info from a form:
            User user = new User();
            PropertyAssigner.Assign(user, Parameters);

            // Save object in your database.

            return Redirect("/user/index/");
        }

        [ValidFor(Method.Post)]
        public IViewData Info()
        {
            return Render();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnException(ExceptionContext context)
        {
            
        }
    }
}