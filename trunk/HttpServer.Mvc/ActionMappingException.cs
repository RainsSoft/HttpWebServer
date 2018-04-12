using System;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Failed to map a controller action.
    /// </summary>
    /// <remarks>
    /// Thrown when controllers are being wired during startup.
    /// </remarks>
    public class ActionMappingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMappingException"/> class.
        /// </summary>
        /// <param name="errorMsg">The error MSG.</param>
        public ActionMappingException(string errorMsg) : base(errorMsg)
        {
        }
    }
}