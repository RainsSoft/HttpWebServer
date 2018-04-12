using NUnit.Framework;
using HttpServer;

namespace HttpServer.Test
{
    [TestFixture]
    public class HttpRequestTest
    {
        HttpRequest _request;


        public HttpRequestTest()
        {
            _request = new HttpRequest();
        }

        public void TestEmptyObject()
        {
            Assert.AreEqual(string.Empty, _request.HttpVersion);
            Assert.AreEqual(string.Empty, _request.Method);
            Assert.AreEqual(HttpHelper.EmptyUri, _request.Uri);
            Assert.AreEqual(null, _request.AcceptTypes);
            Assert.AreEqual(0, _request.Body.Length);
            Assert.AreEqual(0, _request.ContentLength);
            Assert.AreEqual(HttpInput.Empty, _request.QueryString);
            Assert.AreEqual(0, _request.Headers.Count);
        }

        public void TestHeaders()
        {
            _request.AddHeader("connection", "keep-alive");
            _request.AddHeader("content-length", "10");
            _request.AddHeader("content-type", "text/html");
            _request.AddHeader("host", "www.gauffin.com");
            _request.AddHeader("accept", "gzip, text/html, bajs");

            Assert.AreEqual(10, _request.ContentLength);
            Assert.AreEqual("text/html", _request.Headers["content-type"]);
            Assert.AreEqual("www.gauffin.com", _request.Headers["host"]);
            Assert.AreEqual("gzip", _request.AcceptTypes[0]);
            Assert.AreEqual("text/html", _request.AcceptTypes[1]);
            Assert.AreEqual("bajs", _request.AcceptTypes[2]);
        }
    }
}
