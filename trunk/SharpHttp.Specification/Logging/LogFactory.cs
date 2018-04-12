using System;

namespace SharpWeb.Logging
{
    /// <summary>
    /// Factory is used to create new logs in the system.
    /// </summary>
    public static class LogFactory
    {
        private static ILogFactory _factory;

        /// <summary>
        /// Assigns log factory being used.
        /// </summary>
        /// <param name="logFactory">The log factory.</param>
        /// <exception cref="InvalidOperationException">A factory have already been assigned.</exception>
        public static void Assign(ILogFactory logFactory)
        {
            _factory = logFactory;
        }

        /// <summary>
        /// Create a new logger.
        /// </summary>
        /// <returns>Logger for the specified type;</returns>
        public static ILogger CreateLogger<T>()
        {
            return _factory.CreateLogger(typeof(T));
        }
    }
}