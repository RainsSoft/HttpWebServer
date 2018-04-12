using System.Net;
using System.Text;
using HttpServer.Headers;
using HttpServer.Messages;

namespace HttpServer.Mvc.ActionResults
{
    /// <summary>
    /// All actions that are built into the system
    /// </summary>
    public class BuiltinActions
    {
        /// <summary>
        /// Creates a javascript alert.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private ProcessingResult ProcessJavascriptAlert(RequestContext context, IActionResult action)
        {
            var alert = (JavascriptAlert) action;
            context.Response.ContentType.Value = "application/javascript";
            string body = "alert('" + alert.Message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "") + "');";
            byte[] bytes = Encoding.UTF8.GetBytes(body);
            context.Response.Body.Write(bytes, 0, bytes.Length);
            return ProcessingResult.SendResponse;
        }

        /// <summary>
        /// Redirects to another page.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private ProcessingResult ProcessRedirect(RequestContext context, IActionResult action)
        {
            var redirect = (Redirect) action;
            context.Response.Status = HttpStatusCode.Redirect;
            context.Response.Add(new StringHeader("Location", redirect.Location));
            return ProcessingResult.SendResponse;
        }

        /// <summary>
        /// Returns pure string content.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private ProcessingResult ProcessStringContent(RequestContext context, IActionResult action)
        {
            var content = (StringContent) action;
        	context.Response.ContentLength.Value = context.Response.Encoding.GetByteCount(content.Body);
            if (content.ContentType != null)
                context.Response.ContentType.Value = content.ContentType;

            var writer = new ResponseWriter();
            writer.SendHeaders(context.HttpContext, context.Response);
            writer.Send(context.HttpContext, content.Body, context.Response.Encoding);
            context.HttpContext.Stream.Flush();
            return ProcessingResult.Abort;
        }

        /// <summary>
        /// Register all our actions.
        /// </summary>
        /// <param name="provider"></param>
        public void Register(ActionResultProvider provider)
        {
            provider.Register<Redirect>(ProcessRedirect);
            provider.Register<StringContent>(ProcessStringContent);
            provider.Register<JavascriptAlert>(ProcessJavascriptAlert);
            provider.Register<ExecuteJavascript>(ProcessExecuteJavascript);
            provider.Register<StreamResult>(ProcessStream);
        }

        private ProcessingResult ProcessStream(RequestContext context, IActionResult action)
        {
            StreamResult result = (StreamResult) action;
            if (context.Response.ContentType.Value == "text/html")
                context.Response.ContentType.Value = "application/octet-stream";

            context.Response.ContentLength.Value = result.Stream.Length;
            ResponseWriter writer = new ResponseWriter();
            writer.SendHeaders(context.HttpContext, context.Response);

            byte[] buffer = new byte[8196];
            int bytesRead = result.Stream.Read(buffer, 0, buffer.Length);
            while (bytesRead > 0)
            {
                context.HttpContext.Stream.Write(buffer, 0, bytesRead);
                bytesRead = result.Stream.Read(buffer, 0, buffer.Length);
            }

            return ProcessingResult.Abort;
        }

        /// <summary>
        /// Sets Content-Type to "application/javascript" and sends the java script as the body.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private ProcessingResult ProcessExecuteJavascript(RequestContext context, IActionResult action)
        {
            var js = (ExecuteJavascript)action;
            context.Response.ContentType.Value = "application/javascript";
            byte[] bytes = Encoding.UTF8.GetBytes(js.Value);
            context.Response.Body.Write(bytes, 0, bytes.Length);
            return ProcessingResult.SendResponse;
        }
    }
}