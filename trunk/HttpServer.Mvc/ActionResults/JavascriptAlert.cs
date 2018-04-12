namespace HttpServer.Mvc.ActionResults
{
    /// <summary>
    /// Encodes message and displays a JavaScript alert.
    /// </summary>
    /// <remarks>
    /// <para>Typical usage is as a response to a Ajax request.</para>
    /// <para>Content-type will be set to <c>application/javascript</c></para>
    /// </remarks>
    public class JavascriptAlert : IActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavascriptAlert"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public JavascriptAlert(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets message to display
        /// </summary>
        public string Message { get; private set; }
    }
}