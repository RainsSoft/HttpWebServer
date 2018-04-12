using System;
using System.Net;
using System.Reflection;
using HttpServer.Logging;
using HttpServer.Mvc;
using HttpServer.Routing;
using HttpServer.ViewEngine.NHaml;
using HttpListener=HttpServer.HttpListener;

namespace NHaml
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
            server.ViewEngines.Add(new NHamlViewEngine());
            server.Add(HttpListener.Create(IPAddress.Any, 8080));
            server.Add(new SimpleRouter("/", "/user/"));

            // Load controllers and embedded views.
            BootStrapper bootStrapper = new BootStrapper(server);
            bootStrapper.LoadEmbeddedViews(thisAssembly);
            bootStrapper.LoadControllers(thisAssembly);

            server.Start(5);

            // run until you press enter.
            Console.ReadLine();
        }
    }
}