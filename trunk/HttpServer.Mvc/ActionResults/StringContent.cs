using System;

namespace HttpServer.Mvc.ActionResults
{
    /// <summary>
    /// Got the whole body in a string.
    /// </summary>
    public class StringContent : IActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringContent"/> class.
        /// </summary>
        public StringContent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringContent"/> class.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <exception cref="ArgumentNullException"><c>body</c> is <c>null</c>.</exception>
        public StringContent(string body)
        {
            if (body == null)
                throw new ArgumentNullException("body");
            Body = body;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringContent"/> class.
        /// </summary>
        /// <param name="contentType">Type of content.</param>
        /// <param name="body">The body.</param>
        /// <exception cref="ArgumentNullException"><c>body</c> is <c>null</c>.</exception>
        public StringContent(string contentType, string body)
        {
            if (body == null)
                throw new ArgumentNullException("body");
            ContentType = contentType;
            Body = body;
        }

        /// <summary>
        /// Gets or sets body content.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        public string ContentType { get; set; }
    }
}