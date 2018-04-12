namespace SharpWeb.Headers
{
    /// <summary>
    /// Header in a message
    /// </summary>
    /// <remarks>
    /// Important! Each header should override ToString() 
    /// and return it's data correctly formatted as a HTTP header value.
    /// </remarks>
    public interface IHeader
    {
        /// <summary>
        /// Gets header name
        /// </summary>
        string HeaderName { get; }

        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <value>The header value.</value>
        /// <remarks>
        /// Should be formatted as it should be returned to the client.
        /// </remarks>
        string HeaderValue { get; }
    }
}