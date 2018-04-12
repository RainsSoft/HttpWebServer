using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpServer.Headers;
using HttpServer.Headers.Parsers;
using HttpServer.Tools;
using Xunit;

namespace HttpServerTest.Headers
{
    public class CookieHeaderTest
    {
        [Fact]
        public void TestParser()
        {
            CookieCollection c;
            
            CookieParser parser = new CookieParser();
            CookieHeader header = (CookieHeader)parser.Parse("Cookie", new StringReader("name: \"Value\";cookie2: value2;cookieName;last:one"));
            Assert.Equal("Value", header.Cookies["name"].Value);
            Assert.Equal("value2", header.Cookies["cookie2"].Value);
            //Assert.Equal(string.Empty, header.Cookies["cookieName"].Value);
            Assert.Equal("one", header.Cookies["last"].Value);

        }
    }
}
