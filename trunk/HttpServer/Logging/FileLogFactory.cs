using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Logging {
    class FileLogFactory : ILogFactory {
        private readonly ILogFilter _filter;
        private string m_File = string.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogFactory"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public FileLogFactory(string file, ILogFilter filter) {
            m_File = file;
            _filter = filter;
        }

        #region ILogFactory Members

        /// <summary>
        /// Create a new logger.
        /// </summary>
        /// <param name="type">Type that requested a logger.</param>
        /// <returns>Logger for the specified type;</returns>
        /// <remarks>
        /// MUST ALWAYS return a logger. Return <see cref="NullLogWriter"/> if no logging
        /// should be used.
        /// </remarks>
        public ILogger CreateLogger(Type type) {
            return new FileLogger();//type, _filter);
        }

        #endregion
    }
}
