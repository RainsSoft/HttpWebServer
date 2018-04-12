using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer;
using HttpServer.Helpers;
using Xunit;

namespace HttpServerTest.Tools
{
    public class PropertyAssignerTEst
    {
        [Fact]
        public void Test()
        {
            var user = new User();

            ParameterCollection col = new ParameterCollection {{"Name", "Jonas"}, {"Age", "17"}};
            PropertyAssigner.Assign(user, col);
            Assert.Equal("Jonas", user.Name);
            Assert.Equal(17, user.Age);
        }
    }

    public class User
    {
        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public int Age { get; set; }
    }

}
