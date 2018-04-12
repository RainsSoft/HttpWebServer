namespace HttpServer.Mvc.Controllers
{
    [Layout("User")]
    internal class UserController : Controller
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            return null;
        }

        [ValidFor(Method.Get)]
        public string Get()
        {
            return null;
        }

        public IActionResult IndexPost()
        {
            return Redirect("View");
        }
    }
}