using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Headers
{
    //class ExtensionHeaders {
    //}
    /// <summary>
    /// 支持火狐浏览器过期头
    /// </summary>
    public class PragmaHeader : IHeader
    {
        /// <summary>
        /// Header name
        /// </summary>
        public const string NAME = "Pragma";

        #region IHeader Members

        /// <summary>
        /// Gets header name
        /// </summary>
        public string Name {
            get { return NAME; }
        }

        public string HeaderValue {
            get { return "no-cache"; }
        }

        #endregion
    }
    /// <summary>
    /// 支持IE浏览器过期头
    /// </summary>
    public class ExpiresHeader : IHeader
    {

        /// <summary>
        /// Header name
        /// </summary>
        public const string NAME = "Expires";

        #region IHeader Members

        /// <summary>
        /// Gets header name
        /// </summary>
        public string Name {
            get { return NAME; }
        }

        public string HeaderValue {
            get { return "Thu, 01 Jan 1970 00:00:00 GMT"; }
        }

        #endregion
    }
    /// <summary>
    /// 发送文件到客户端头
    /// Content-disposition: attachment;filename=Setup_1.3.0.5_ZJ.exe
    /// </summary>
    public class SendFileHeader : IHeader
    {
        #region IHeader 成员
        public SendFileHeader(string filename) {
            this.FileName = filename;
        }
        public string Name {
            get { return "Content-disposition"; }
        }

        public string HeaderValue {
            get { return "attachment;filename=" + FileName; }
        }
        public string FileName {
            get;
            set;
        }
        #endregion
    }

}
