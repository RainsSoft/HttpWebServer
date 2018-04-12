using HttpServer.Resources;
using Xunit;

namespace HttpServerTest.Resources
{
    public class EmbeddedResourcesTest
    {
        private EmbeddedResourceLoader _resource;

        public EmbeddedResourcesTest()
        {
            _resource = new EmbeddedResourceLoader();
        }

        [Fact]
        public void Add()
        {
            _resource.Add("/users/", GetType().Assembly, GetType().Namespace + ".Views",
                           GetType().Namespace + ".Views.MyFile.xml.spark");
        }

        [Fact]
        private void AutoFind()
        {
            _resource = new EmbeddedResourceLoader("/", GetType().Assembly, GetType().Namespace + ".Views");
        }
    }
}