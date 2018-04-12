using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HttpServer.MVC2
{
    public class MvcServer
    {
        private HttpListener _listener;

        public void Start()
        {
            _listener.Start();
        }
    }
}
