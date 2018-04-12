namespace HttpServer.Mvc.ActionResults
{
    /// <summary>
    /// Send back a binary buffer. 
    /// </summary>
    /// <remarks>
    /// Content-Type will be set to <c>application/octet-stream</c> if it has not been specified.
    /// </remarks>
    public class BinaryResult : IActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryResult"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public BinaryResult(byte[] buffer)
        {
            Buffer = buffer;
        }

        /// <summary>
        /// Gets buffer to send
        /// </summary>
        public byte[] Buffer { get; private set; }
    }
}