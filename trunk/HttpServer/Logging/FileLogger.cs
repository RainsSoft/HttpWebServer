using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Logging {
    class FileLogger:ILogger {
        #region ILogger 成员

        public void Debug(string message) {
            throw new NotImplementedException();
        }

        public void Debug(string message, Exception exception) {
            throw new NotImplementedException();
        }

        public void Error(string message) {
            throw new NotImplementedException();
        }

        public void Error(string message, Exception exception) {
            throw new NotImplementedException();
        }

        public void Fatal(string message) {
            throw new NotImplementedException();
        }

        public void Fatal(string message, Exception exception) {
            throw new NotImplementedException();
        }

        public void Info(string message) {
            throw new NotImplementedException();
        }

        public void Info(string message, Exception exception) {
            throw new NotImplementedException();
        }

        public void Trace(string message) {
            throw new NotImplementedException();
        }

        public void Trace(string message, Exception exception) {
            throw new NotImplementedException();
        }

        public void Warning(string message) {
            throw new NotImplementedException();
        }

        public void Warning(string message, Exception exception) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
