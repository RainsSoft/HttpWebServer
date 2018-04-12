using System;
using System.Collections.Generic;
using System.Text;
using HttpServer.Test.Controllers;
using HttpServer.Test.FormDecoders;
using HttpServer.Test.Renderers;

namespace HttpServer.Test
{
    class Program
    {


        static void Main(string[] args)
        {
            //HamlTest ytest = new HamlTest();
            //ytest.TestLayout();

            HttpServerLoadTests tests = new HttpServerLoadTests();
            tests.Test();
        }
    }
}
