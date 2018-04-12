using System;
using System.Collections.Generic;
using System.Text;
using HttpServer.Controllers;

namespace HttpServer.Test.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    class MyController : RequestController
    {
        public string HelloWorld()
        {
            return "HelloWorld";
        }

        [RawHandler]
        public void Raw()
        {
            Response.SendHeaders();

            byte[] mybytes = Encoding.ASCII.GetBytes("Hello World");
            Response.SendBody(mybytes, 0, mybytes.Length);
        }

        public override object Clone()
        {
            return new MyController();
        }
    }
}
