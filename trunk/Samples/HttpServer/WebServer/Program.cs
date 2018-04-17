using System;
using System.Net;
using HttpServer;
using HttpServer.BodyDecoders;
using HttpServer.Logging;
using HttpServer.Messages;
using HttpServer.Modules;
using HttpServer.Resources;
using HttpServer.Routing;
using HttpListener = HttpServer.HttpListener;

namespace HttpServerSample
{
    internal class Program
    {
        static private ConsoleAndTextLogger Logger;
        static private ResponseWriter m_RspsWriter;
        private static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Console.Title = "Web Server";


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  Rains Soft Web Server");
            Console.WriteLine("      Rains Soft");
            Console.WriteLine("  http://www.mobanhou.com");
            Console.WriteLine();
            Console.ResetColor();
            int i = 0;
            while (true) {
                if (i > 9) {
                    Console.WriteLine(".");
                    break;
                }
                else {
                    Console.Write(".");
                    i++;
                }
                System.Threading.Thread.Sleep(500);
            }
          
            var filter = new LogFilter();
            filter.AddStandardRules();
            var log = new ConsoleLogFactory(filter);            
            LogFactory.Assign(log);
            Logger=LogFactory.CreateLogger(log.GetType()) as ConsoleAndTextLogger;
            Logger.Info("create server");
            // create a server.
            var server = new Server();
            
            // same as previous example.
            var module = new FileModule();
            module.Resources.Add(new FileResources("/", Environment.CurrentDirectory + "\\files\\"));
            server.Add(module);
            server.Add(new CustomHttpModule());
            server.RequestReceived += OnRequest;
            server.Add(new MultiPartDecoder());

            // use one http listener.
            server.Add(HttpListener.Create(IPAddress.Any, 8085));
            server.Add(new SimpleRouter("/", "/index.html"));
            Logger.Info("start server");
            // start server, can have max 5 pending accepts.
            server.Start(5);
            Console.Beep();
            Console.ReadLine();
        }

        private static void OnRequest(object sender, RequestEventArgs e)
        {
            if (e.Request.Method == Method.Post)
            {
                
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Console.WriteLine("出现了未处理的错误！" + e.ExceptionObject.ToString());
            Logger.Write( LogLevel.Error,e.ExceptionObject.ToString());
        }
    }
}