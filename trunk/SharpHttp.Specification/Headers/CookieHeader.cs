using System;
using System.Text;
using SharpWeb.Messages;
using SharpWeb.Messages;

namespace SharpWeb.Headers
{
    /// <summary>
    /// Contents of a cookie header.
    /// </summary>
    public class CookieHeader : IHeader
    {
        public const string NAME = "Cookie";

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieHeader"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="ArgumentNullException"><c>collection</c> is <c>null</c>.</exception>
        public CookieHeader(RequestCookieCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            Cookies = collection;
        }

        /// <summary>
        /// Gets cookie collection
        /// </summary>
        public RequestCookieCollection Cookies { get; private set; }

        #region IHeader Members

        /// <summary>
        /// Gets header name
        /// </summary>
        public string HeaderName
        {
            get { return NAME; }
        }

        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <value>The header value.</value>
        /// <remarks>
        /// Should be formatted as it should be returned to the client.
        /// </remarks>
        public string HeaderValue
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (RequestCookie cooky in Cookies)
                {
                    sb.AppendFormat("{0}={1};", cooky.Name, cooky.Value);
                }
                return sb.ToString();
            }
        }

        #endregion
    }
}