using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HttpServer;
using HttpListener=HttpServer.HttpListener;

namespace SecureListener
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var certificate = new X509Certificate2("C:\\OpenSSL\\bin\\newcert.p12", "test");

            // We do the cast since we want to specify UseClientCert
            var listener = (SecureHttpListener) HttpListener.Create(IPAddress.Any, 8080, certificate);
            listener.UseClientCertificate = true;
            listener.RequestReceived += OnRequest;
            listener.Start(5);

            Console.ReadLine();
        }

        private static void OnRequest(object sender, RequestEventArgs e)
        {
            // Write info to the buffer.
            byte[] buffer = Encoding.Default.GetBytes("Hello secure world");
            e.Response.Body.Write(buffer, 0, buffer.Length);
        }
    }
}