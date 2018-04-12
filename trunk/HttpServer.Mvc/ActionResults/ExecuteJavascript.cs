namespace HttpServer.Mvc.ActionResults
{
    /// <summary>
    /// Send back a JavaScript and execute it.
    /// </summary>
    /// <remarks>
    /// A JavaScript without any HTML tags should be used.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyController : Controller
    /// {
    ///   public IActionResult Remove()
    ///   {
    ///     //[..]
    ///     
    ///     // Update table using jQuery.
    ///     if (Request.IsAjax)
    ///         return new ExecuteJavascript("$('.table').loadJson('/users/list/');");
    ///     return Redirect('/users/list/');
    ///   }
    /// }
    /// </code>
    /// </example>
    public class ExecuteJavascript : IActionResult
    {
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteJavascript"/> class.
        /// </summary>
        /// <param name="javaScript">JavaScript to execute. Should not be wrapped in any HTML tags.</param>
        public ExecuteJavascript(string javaScript)
        {
            _value = javaScript;
        }

        /// <summary>
        /// Gets JavaScript to execute
        /// </summary>
        public string Value
        {
            get { return _value; }
        }
    }
}