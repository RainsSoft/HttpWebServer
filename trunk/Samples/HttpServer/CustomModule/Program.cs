using System;
using System.Net;
using HttpServer;
using HttpServer.Modules;
using HttpServer.Resources;
using HttpListener = HttpServer.HttpListener;

namespace CustomModule
{
    class Program
    {
        static void Main(string[] args)
        {
            // create a server.
            var server = new Server();

            // same as previous example.
            var module = new FileModule();
            module.Resources.Add(new FileResources("/", Environment.CurrentDirectory + "\\files\\"));
            server.Add(module);

            // use one http listener.
            server.Add(HttpListener.Create(IPAddress.Any, 8085));

            // add our own module.
            server.Add(new Streamer());


            // start server, can have max 5 pending accepts.
            server.Start(5);

            Console.ReadLine();
        }
    }
}
