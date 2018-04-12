using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Mvc.Routing
{
    public class RoutingContext
    {
        public RoutingContext(IRequest request)
        {
            Request = request;
        }

        public IRequest Request { get; private set; }
    }
}
