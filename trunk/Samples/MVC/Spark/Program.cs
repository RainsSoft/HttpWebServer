using System;
using System.Net;
using System.Reflection;
using HttpServer.Logging;
using HttpServer.Mvc;
using HttpServer.Resources;
using HttpServer.Routing;
using HttpServer.ViewEngine.Spark;
using HttpListener=HttpServer.HttpListener;

namespace Spark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Log everything to console.
            LogFactory.Assign(new ConsoleLogFactory(null));

            Assembly thisAssembly = typeof (Program).Assembly;

            // create a MVC web server.
            var server = new MvcServer();
            server.ViewEngines.Add(new SparkEngine());
            server.Add(HttpListener.Create(IPAddress.Any, 8080));
            server.Add(new SimpleRouter("/", "/user/"));

            // Load controllers and embedded views.
            BootStrapper bootStrapper = new BootStrapper(server);
            bootStrapper.LoadEmbeddedViews(thisAssembly);
            bootStrapper.LoadControllers(thisAssembly);

            // And run the server.
            server.Start(5);

            // run until you press enter.
            Console.ReadLine();
        }
    }
}