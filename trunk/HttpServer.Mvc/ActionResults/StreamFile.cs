using System.IO;

namespace HttpServer.Mvc.ActionResults
{
    /// <summary>
    /// Sends a stream to the client
    /// </summary>
    /// <remarks>
    /// Stream.Length must be correct in order for this action to work properly.
    /// </remarks>
    public class StreamResult : IActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamResult"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StreamResult(Stream stream)
        {
            Stream = stream;
        }

        /// <summary>
        /// Gets stream to send
        /// </summary>
        public Stream Stream { get; private set; }
    }
}