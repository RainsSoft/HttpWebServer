using HttpServer.Mvc;
using HttpServer.Mvc.Controllers;

namespace NHaml.Controllers
{
    public class UserController : Controller
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
            ViewData["whom"] = "World";
            return Render();
        }

        public IViewData InfoPost()
        {
            return Render();
        }
    }
}