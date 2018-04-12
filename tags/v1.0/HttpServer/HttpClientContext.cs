using System;
using System.Net;
using System.Net.Sockets;

namespace HttpServer
{
    /// <summary>
    /// Contains a connection to a browser/client.
    /// </summary>
    public interface HttpClientContext
    {
        /// <summary>
        /// Using SSL or other encryption method.
        /// </summary>
        bool Secured { get; }

        /// <summary>
        /// Disconnect from client
        /// </summary>
        /// <param name="error">error to report in the <see cref="ClientDisconnectedHandler"/> delegate.</param>
        void Disconnect(SocketError error);

        /// <summary>
        /// Send a response.
        /// </summary>
        /// <param name="httpVersion">Either HttpHelper.HTTP10 or HttpHelper.HTTP11</param>
        /// <param name="statusCode">http status code</param>
        /// <param name="reason">reason for the status code.</param>
        /// <param name="body">html body contents, can be null or empty.</param>
        /// <exception cref="ArgumentException">If httpVersion is invalid.</exception>
        void Respond(string httpVersion, HttpStatusCode statusCode, string reason, string body);

        /// <summary>
        /// Send a response.
        /// </summary>
        /// <param name="httpVersion">Either HttpHelper.HTTP10 or HttpHelper.HTTP11</param>
        /// <param name="statusCode">http status code</param>
        /// <param name="reason">reason for the status code.</param>
        void Respond(string httpVersion, HttpStatusCode statusCode, string reason);

        /// <summary>
        /// Send a response.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        void Respond(string body);

        /// <summary>
        /// send a whole buffer
        /// </summary>
        /// <param name="buffer">buffer to send</param>
        /// <exception cref="ArgumentNullException"></exception>
        void Send(byte[] buffer);

        /// <summary>
        /// Send data using the stream
        /// </summary>
        /// <param name="buffer">Contains data to send</param>
        /// <param name="offset">Start position in buffer</param>
        /// <param name="size">number of bytes to send</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void Send(byte[] buffer, int offset, int size);
    }

}
