using System;

namespace SharpWeb.Headers
{
    /// <summary>
    /// Used to store all headers that that aren't recognized.
    /// </summary>
    public class StringHeader : IHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringHeader"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public StringHeader(string name, string value)
        {
            HeaderName = name;
            HeaderValue = value;
        }

        #region IHeader Members

        /// <summary>
        /// Gets header name
        /// </summary>
        public string HeaderName { get; private set; }

        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <value>The header value.</value>
        /// <remarks>
        /// Should be formatted as it should be returned to the client.
        /// </remarks>
        public string HeaderValue
        {
            get; set;
        }

        #endregion

 
    }
}