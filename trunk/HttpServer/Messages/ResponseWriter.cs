using System;
using System.IO;
using System.Text;
using HttpServer.Headers;
using HttpServer.Logging;

namespace HttpServer.Messages
{
    /// <summary>
    /// Used to send a response back to the client.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Writes a <see cref="IResponse"/> object into a stream.
    /// </para>
    /// <para>
    /// Important! ResponseWriter do not throw any exceptions. Instead it just logs them and
    /// let them die peacefully. This is since the response writer is used from
    /// catch blocks here and there.
    /// </para>
    /// </remarks>
    public class ResponseWriter
    {
        private ILogger _logger = LogFactory.CreateLogger(typeof (ResponseWriter));

        /// <summary>
        /// Sends response using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="response">The response.</param>
        public void Send(IHttpContext context, IResponse response)
        {
            SendHeaders(context, response);
            SendBody(context, response.Body);
            
            try
            {
                context.Stream.Flush();
            }
            catch(Exception err)
            {
                _logger.Error("Failed to flush context stream.", err);
            }
        }       
        /// <summary>
        /// 发送Xml字符串到客户端。采用UTF8编码。
        /// 该次发送设定了Content-Type,ContentLength，一次性发送完毕。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="str"></param>
        public void SendXmlString(IHttpContext context, string str) {
            byte[] buf = Encoding.UTF8.GetBytes(str);
            if (context.Response.Headers["Content-Type"] != null) {

            }
            Headers.ContentTypeHeader head = new ContentTypeHeader("Content-Type");
            head.Value = "text/xml; charset=utf-8";
            context.Response.ContentType = head;

            context.Response.ContentLength.Value = buf.Length;
            //context.Response.Add("Pragma", new PragmaHeader());
            //context.Response.Add("Expires", new ExpiresHeader());
            context.Response.Body.Write(buf, 0, buf.Length);

            Send(context, context.Response);
        }
        public void SendJson(IHttpContext context, string jsonString) {
            byte[] buf = Encoding.UTF8.GetBytes(jsonString);
            if (context.Response.Headers["Content-Type"] != null) {

            }
            Headers.ContentTypeHeader head = new ContentTypeHeader("Content-Type");
            head.Value = "text/json; charset=utf-8";
            context.Response.ContentType = head;
            context.Response.ContentLength.Value = buf.Length;
            //context.Response.Add("Pragma", new PragmaHeader());
            //context.Response.Add("Expires", new ExpiresHeader());
            context.Response.Body.Write(buf, 0, buf.Length);
            Send(context, context.Response);
        }
        /// <summary>
        /// Converts and sends a string.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <param name="encoding">Encoding used to transfer string</param>
        public void Send(IHttpContext context, string data, Encoding encoding)
        {
            try
            {
                byte[] buffer = encoding.GetBytes(data);
                _logger.Debug("Sending " + buffer.Length + " bytes.");
                if (data.Length < 4000)
                    _logger.Trace(data);
                context.Stream.Write(buffer, 0, buffer.Length);
            }
            catch(Exception err)
            {
                _logger.Error("Failed to send data through context stream.", err);
            }
        }

        /// <summary>
        /// Send a body to the client
        /// </summary>
        /// <param name="context">Context containing the stream to use.</param>
        /// <param name="body">Body to send</param>
        public void SendBody(IHttpContext context, Stream body)
        {
            try
            {
                body.Flush();
                body.Seek(0, SeekOrigin.Begin);
                var buffer = new byte[4196];
                int bytesRead = body.Read(buffer, 0, 4196);
                while (bytesRead > 0)
                {
                    context.Stream.Write(buffer, 0, bytesRead);
                    bytesRead = body.Read(buffer, 0, 4196);
                }
            }
            catch(Exception err)
            {
                _logger.Error("Failed to send body through context stream.", err);
            }
        }

        /// <summary>
        /// Send all headers to the client
        /// </summary>
        /// <param name="response">Response containing call headers.</param>
        /// <param name="context">Content used to send headers.</param>
        public void SendHeaders(IHttpContext context, IResponse response)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1} {2}\r\n", response.HttpVersion, (int) response.Status, response.Reason);

			// replace content-type name with the actual one used.
        	//response.ContentType.Parameters["charset"] = response.Encoding.WebName;

            // go through all property headers.
            sb.AppendFormat("{0}: {1}\r\n", response.ContentType.Name, response.ContentType);
            sb.AppendFormat("{0}: {1}\r\n", response.ContentLength.Name, response.ContentLength);
            sb.AppendFormat("{0}: {1}\r\n", response.Connection.Name, response.Connection);

            if (response.Cookies != null && response.Cookies.Count > 0)
            {
                //Set-Cookie: <name>=<value>[; <name>=<value>][; expires=<date>][; domain=<domain_name>][; path=<some_path>][; secure][; httponly]
                foreach (ResponseCookie cookie in response.Cookies)
                {
                    sb.Append("Set-Cookie: ");
                    sb.Append(cookie.Name);
                    sb.Append("=");
                    sb.Append(cookie.Value ?? string.Empty);

                    if (cookie.Expires > DateTime.MinValue)
                        sb.Append(";expires=" + cookie.Expires.ToString("R"));
                    if (!string.IsNullOrEmpty(cookie.Path))
                        sb.AppendFormat(";path={0}", cookie.Path);
                    sb.Append("\r\n");
                }
            }
            context.Response.Add("Pragma", new PragmaHeader());
            context.Response.Add("Expires", new ExpiresHeader());
            foreach (IHeader header in response)
                sb.AppendFormat("{0}: {1}\r\n", header.Name, header.HeaderValue);

            sb.Append("\r\n");
            Send(context, sb.ToString(), response.Encoding);
            HeadersSent(this, EventArgs.Empty);
        }
        /// <summary>
        /// 发送文本文件。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        public void SendFile(IHttpContext context, IResponse response, Stream stream) {
            try {
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                response.ContentLength.Value = stream.Length;
                if (context.Response.Encoding is UTF8Encoding) {
                    if (context.Response.ContentType.Value.StartsWith("text/plain")
                        || context.Response.ContentType.Value.StartsWith("text/html")
                        || context.Response.ContentType.Value.StartsWith("text/xml")) {

                        stream.Position = 0;
                        //跳过前面三个字节的BOM
                        if (stream.ReadByte() == 0xEF && stream.ReadByte() == 0xBB && stream.ReadByte() == 0xBF) {
                            stream.Seek(3, SeekOrigin.Begin);
                            response.ContentLength.Value = stream.Length - 3;
                        }
                    }
                }
                //SendFileHeader sf = new SendFileHeader("test.csv");
                //context.Response.Add("Content-disposition",sf);
                SendHeaders(context, response);

                var buffer = new byte[4196];
                int bytesRead = stream.Read(buffer, 0, 4196);
                while (bytesRead > 0) {
                    context.Stream.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, 4196);
                }
            }
            catch (Exception err) {
                _logger.Error("Failed to send body through context stream.", err);
            }
        }
        /// <summary>
        /// 发送文件 加上文件头UTF8_BOM:0xEF 0xBB 0xBF
        /// </summary>
        /// <param name="context"></param>
        /// <param name="response"></param>
        /// <param name="attachmentname"></param>
        /// <param name="stream"></param>
        /// <param name="bom">UTF8_BOM:0xEF 0xBB 0xBF</param>
        public void SendFile(IHttpContext context, IResponse response, string attachmentname, Stream stream, byte[] bom) {
            //stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            int addBytes = (bom != null ? bom.Length : 0);
            response.ContentLength.Value = stream.Length + addBytes;
            //
            string oldctvalue = response.ContentType.Value;
            response.ContentType.Value = "application/octet-stream";

            //
            string fname = string.Empty;
            //如果是ie,那就要utf8编码
            if (context.Response.Headers["User-Agent"] != null && context.Response.Headers["User-Agent"].HeaderValue != null) {
                if (context.Response.Headers["User-Agent"].HeaderValue.IndexOf("msie", StringComparison.OrdinalIgnoreCase) > -1) {
                    fname = System.Web.HttpUtility.UrlEncode(attachmentname, Encoding.UTF8);
                }
                else {
                    fname = attachmentname;
                }
            }
            context.Response.Add("Content-disposition", new SendFileHeader(fname));
            //if (context.Response.Encoding is UTF8Encoding) {
            //    stream.Position = 0;
            //    //跳过前面三个字节的BOM
            //    if (stream.ReadByte() == 0xEF && stream.ReadByte() == 0xBB && stream.ReadByte() == 0xBF) {
            //        stream.Seek(3, SeekOrigin.Begin);
            //        response.ContentLength.Value = stream.Length - 3;
            //    }
            //}
            SendHeaders(context, response);
            var buffer = new byte[4196];
            int bytesRead = stream.Read(buffer, 0, 4196);
            bool addbom = (bom != null && bom.Length > 0);
            while (bytesRead > 0) {
                if (addbom) {
                    addbom = false;
                    context.Stream.Write(bom, 0, bom.Length);
                }
                context.Stream.Write(buffer, 0, bytesRead);
                bytesRead = stream.Read(buffer, 0, 4196);
            }
            response.ContentType.Value = oldctvalue;
        }

    
        public void SendErrorPage(IHttpContext context, IResponse response, Exception exception)
        {
            string htmlTemplate = @"<html>
    <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
        <title>{1}</title>
    </head>
    <body>
        <h1>{0} - {1}</h1>
        <pre>{2}</pre>
    </body>
</html>";

            var body = string.Format(htmlTemplate,
                                     (int) response.Status,
                                     response.Reason,
                                     exception);
            byte[] bodyBytes = response.Encoding.GetBytes(body);
            response.Body.Write(bodyBytes, 0, bodyBytes.Length);
            Send(context, response);
        }

        public static event EventHandler HeadersSent = delegate { };
    }
}