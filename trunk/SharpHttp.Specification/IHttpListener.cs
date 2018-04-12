using System;
using System.Net;
using System.Net.Sockets;

namespace SharpWeb
{
    /// <summary>
    /// Interface declaring what a HTTP listener can do
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface IHttpListener
    {
        /// <summary>
        /// Gets listener address.
        /// </summary>
        IPAddress Address { get; }

        /// <summary>
        /// Gets if listener is secure.
        /// </summary>
        bool IsSecure { get; }

        /// <summary>
        /// Gets if listener have been started.
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Gets listening port.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Gets or sets maximum of bytes that the body can have
        /// </summary>
        /// <value>The body length limit.</value>
        /// <remarks>
        /// Used when a 100-continue header is received to determine if 
        /// the client can continue with the body.
        /// </remarks>
        long MaxBodyLength { get; set; }

        /// <summary>
        /// Start listener.
        /// </summary>
        /// <param name="backLog">Number of pending accepts.</param>
        /// <remarks>
        /// Make sure that you are subscribing on <see cref="RequestReceived"/> first.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Listener have already been started.</exception>
        /// <exception cref="SocketException">Failed to start socket.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Invalid port number.</exception>
        void Start(int backLog);

        /// <summary>
        /// Stop listener.
        /// </summary>
        void Stop();

        /// <summary>
        /// A unhandled exception have been caught.
        /// </summary>
        event EventHandler<ExceptionEventArgs> ExceptionThrown;


        /// <summary>
        /// A new request have been received.
        /// </summary>
        event EventHandler<RequestEventArgs> RequestReceived;

        /// <summary>
        /// Client asks if he may continue.
        /// </summary>
        /// <remarks>
        /// If the body is too large or anything like that you should respond <see cref="HttpStatusCode.ExpectationFailed"/>.
        /// </remarks>
        event EventHandler<RequestEventArgs> ContinueResponseRequested;
    }
}