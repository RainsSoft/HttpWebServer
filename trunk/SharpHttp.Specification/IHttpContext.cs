using System.IO;
using System.Net;
using System.Net.Security;

namespace SharpWeb
{
    /// <summary>
    /// Context that received a HTTP request.
    /// </summary>
    public interface IHttpContext
    {
        /// <summary>
        /// Gets if current context is using a secure connection.
        /// </summary>
        bool IsSecure { get; }

        /// <summary>
        /// Gets remote end point
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Gets stream used to send/receive data to/from remote end point.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The stream can be any type of stream, do not assume that it's a network
        /// stream. For instance, it can be a <see cref="SslStream"/> or a ZipStream.
        /// </para>
        /// </remarks>
        Stream Stream { get; }

        /// <summary>
        /// Close the context
        /// </summary>
        void Close();
    }
}