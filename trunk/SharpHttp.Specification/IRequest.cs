using System;
using SharpWeb.Headers;

namespace SharpWeb
{
    /// <summary>
    /// Request sent to a HTTP server.
    /// </summary>
    public interface IRequest : IMessage
    {
        /// <summary>
        /// Gets or sets connection header.
        /// </summary>
        ConnectionHeader Connection { get; }

        /// <summary>
        /// Gets cookies.
        /// </summary>
        IHttpFileCollection Cookies { get; }

        /// <summary>
        /// Gets all uploaded files.
        /// </summary>
        IHttpFileCollection Files { get; }

        /// <summary>
        /// Gets form parameters.
        /// </summary>
        IParameterCollection Form { get; }

        /// <summary>
        /// Gets or sets HTTP version.
        /// </summary>
        string HttpVersion { get; set; }

        /// <summary>
        /// Gets if request is an Ajax request.
        /// </summary>
        bool IsAjax { get; }


        /// <summary>
        /// Get a header 
        /// </summary>
        /// <typeparam name="T">Type that it should be cast to</typeparam>
        /// <param name="headerName">Name of header</param>
        /// <returns>Header if found and casted properly; otherwise <c>null</c>.</returns>
        T Get<T>(string headerName) where T : class, IHeader;

        /// <summary>
        /// Gets or sets HTTP method.
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// Gets query string and form parameters
        /// </summary>
        IParameterCollection Parameters { get; }

        /// <summary>
        /// Gets query string.
        /// </summary>
        IParameterCollection QueryString { get; }

        /// <summary>
        /// Gets requested URI.
        /// </summary>
        Uri Uri { get; set; }
    }
}