using System.Net;
using System.Text;
using System.Threading;
using HttpServer;
using HttpServer.Headers;
using HttpListener=HttpServer.HttpListener;

namespace HttpListenerSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HttpListener listener = HttpListener.Create(IPAddress.Any, 8089);
            listener.RequestReceived += OnRequest;
            listener.Start(5);
            Thread.Sleep(9000000);
        }

        private static void OnRequest(object sender, RequestEventArgs e)
        {
            e.Response.Connection.Type = ConnectionType.Close;
            byte[] buffer = Encoding.UTF8.GetBytes("<html><body>Hello wordl!</body></html>");
            e.Response.Body.Write(buffer, 0, buffer.Length);
        }
    }
}