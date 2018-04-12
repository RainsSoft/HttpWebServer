using System;
using System.Net;
using HttpServer;
using HttpServer.BodyDecoders;
using HttpServer.Logging;
using HttpServer.Modules;
using HttpServer.Resources;
using HttpServer.Routing;
using HttpListener=HttpServer.HttpListener;

namespace HttpServerSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filter = new LogFilter();
            filter.AddStandardRules();
            //LogFactory.Assign(new ConsoleLogFactory(filter));

            // create a server.
            var server = new Server();
            
            // same as previous example.
            var module = new FileModule();
            module.Resources.Add(new FileResources("/", Environment.CurrentDirectory + "\\files\\"));
            server.Add(module);
            server.RequestReceived += OnRequest;
            server.Add(new MultiPartDecoder());

            // use one http listener.
            server.Add(HttpListener.Create(IPAddress.Any, 8085));
            server.Add(new SimpleRouter("/", "/index.html"));

            // start server, can have max 5 pending accepts.
            server.Start(5);

            Console.ReadLine();
        }

        private static void OnRequest(object sender, RequestEventArgs e)
        {
            if (e.Request.Method == Method.Post)
            {
                
            }
        }
    }
}