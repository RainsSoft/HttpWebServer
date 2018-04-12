using System.Web;
using HttpServer;
using HttpServer.Tools;
using Xunit;

namespace HttpServerTest.Tools
{
    public class QueryStringParserTest
    {
        [Fact]
        public void SingleItem()
        {
            IParameterCollection items = UrlParser.Parse("item");
            Assert.Equal(1, items.Count);
            Assert.Equal(string.Empty, items["item"]);
        }

        [Fact]
        public void SingleItemWithEncodedValue()
        {
            IParameterCollection items = UrlParser.Parse("item=" + HttpUtility.UrlEncode("some&shit! "));
            Assert.Equal(1, items.Count);
            Assert.Equal("some&shit! ", items["item"]);
        }

        [Fact]
        public void SingleItemWithValue()
        {
            IParameterCollection items = UrlParser.Parse("item=hello");
            Assert.Equal(1, items.Count);
            Assert.Equal("hello", items["item"]);
        }
    }
}