using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HttpServer.Mvc.Routing
{
    public class RouteResult
    {
        public Type ControllerType { get; set; }
        public MethodInfo Action { get; set; }
        public string ActionName { get; set; }
        public string ControllerUri { get; set; }
    }
}
