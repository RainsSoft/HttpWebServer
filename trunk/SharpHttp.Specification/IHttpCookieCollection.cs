using System;
using System.Collections.Generic;

namespace SharpWeb.Messages
{
    public interface IHttpCookieCollection : IEnumerable<IHttpCookie>
    {
        /// <summary>
        /// Gets the count of cookies in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the cookie of a given identifier (<c>null</c> if not existing).
        /// </summary>
        IHttpCookie this[string id] { get; }

        /// <summary>
        /// Adds a cookie in the collection.
        /// </summary>
        /// <param name="cookie">cookie to add</param>
        /// <exception cref="ArgumentNullException">cookie is <c>null</c></exception>
        /// <exception cref="ArgumentException">Name must be specified.</exception>
        void Add(IHttpCookie cookie);

        /// <summary>
        /// Remove all cookies.
        /// </summary>
        void Clear();

        /// <summary>
        /// Remove a cookie from the collection.
        /// </summary>
        /// <param name="cookieName">Name of cookie.</param>
        void Remove(string cookieName);
    }
}