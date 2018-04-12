using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeb.Headers
{
    public class KeepAliveHeader : IHeader
    {
        public const string NAME = "Keep-Alive";

        public KeepAliveHeader()
        {
            Timeout = 10;
            Max = 100;
        }

        /// <summary>
        /// Gets or sets number of seconds that the connection should remain open
        /// </summary>
        /// <value>Default is 10 seconds</value>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets maximum number of seconds that the connection can be open.
        /// </summary>
        /// <value>Default is 100 seconds</value>
        public int Max { get; set; }

        /// <summary>
        /// Gets header name
        /// </summary>
        public string HeaderName
        {
            get { return NAME; }
        }


        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <value>The header value.</value>
        /// <remarks>
        /// Should be formatted as it should be returned to the client.
        /// </remarks>
        public string HeaderValue
        {
            get { return string.Format("timeout={0}, max={1}", Timeout, Max); }
        }
    }
}
