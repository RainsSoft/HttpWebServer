using HttpServer;
using HttpServer.Tools;
using Xunit;

namespace HttpServerTest.Tools
{
    public class NestedQueryStringTest
    {
        [Fact]
        public void Complex()
        {
            ArrayParameterCollection q =
                Parse(
                    "users[name]=value&users[lastName]=Gauffin&simple=isHere&users[father[name]]=Stefan&user[alias]=Verifier&user[alias]=StorBruno");

            Assert.Equal("value", q["users"]["name"].Value);
            Assert.Equal("Gauffin", q["users"]["lastName"].Value);
            Assert.Equal("Stefan", q["users"]["father"]["name"].Value);
            Assert.Equal("Verifier", q["user"]["alias"].Values[0]);
            Assert.Equal("StorBruno", q["user"]["alias"].Values[1]);
        }

        [Fact]
        public void MultipleValues()
        {
            ArrayParameterCollection q = Parse("user[alias]=Verifier&user[alias]=StorBruno");
            Assert.Equal("Verifier", q["user"]["alias"].Values[0]);
            Assert.Equal("StorBruno", q["user"]["alias"].Values[1]);
        }

        [Fact]
        public void OneNesting()
        {
            ArrayParameterCollection q = Parse("users[name]=value");
            Assert.Equal("value", q["users"]["name"].Value);
        }

        private ArrayParameterCollection Parse(string queryString)
        {
            var values = UrlParser.Parse(queryString);
            return new ArrayParameterCollection(values);
        }
    }
}