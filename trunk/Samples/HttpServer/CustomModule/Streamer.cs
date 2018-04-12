using System.IO;
using HttpServer;
using HttpServer.Messages;
using HttpServer.Modules;

namespace CustomModule
{
    /// <summary>
    /// Example on how to stream a file
    /// </summary>
    public class Streamer : IModule
    {
        /// <summary>
        /// Process a request.
        /// </summary>
        /// <param name="context">Request information</param>
        /// <returns>What to do next.</returns>
        public ProcessingResult Process(RequestContext context)
        {
            // should add information about the file here.

            // Lets send the header.
            ResponseWriter generator = new ResponseWriter();
            generator.SendHeaders(context.HttpContext, context.Response);

            // loop through file contents.
            BinaryWriter writer = new BinaryWriter(context.HttpContext.Stream);
            


            // Abort is needed since we've used the network
            // stream directly. Else the framework will try to use the
            // response object + network stream to send another response.
            return ProcessingResult.Abort; 
        }
    }
}
