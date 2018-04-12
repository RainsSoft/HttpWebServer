namespace HttpServer.Mvc
{
    /// <summary>
    /// Could not find the specified view.
    /// </summary>
    public class ViewNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewNotFoundException"/> class.
        /// </summary>
        /// <param name="viewUri">The view URI.</param>
        /// <param name="errorMessage">The error message.</param>
        public ViewNotFoundException(string viewUri, string errorMessage) : base(errorMessage)
        {
            ViewUri = viewUri;
        }

        /// <summary>
        /// Gets URI to view that was not found.
        /// </summary>
        public string ViewUri { get; private set; }
    }
}