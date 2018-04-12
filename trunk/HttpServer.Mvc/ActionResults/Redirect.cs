namespace HttpServer.Mvc.ActionResults
{
    /// <summary>
    /// Redirect to another page.
    /// </summary>
    public class Redirect : IActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Redirect"/> class.
        /// </summary>
        /// <param name="uri">Uri to redirect to.</param>
        /// <remarks>
        /// Include "http://" in URI to redirect to another site.
        /// </remarks>
        public Redirect(string uri)
        {
            Location = uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Redirect"/> class.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public Redirect(string controllerName, string actionName)
        {
            Location = "/" + controllerName + "/" + actionName;
        }

        /// <summary>
        /// Gets location to redirect to.
        /// </summary>
        public string Location { get; private set; }
    }
}