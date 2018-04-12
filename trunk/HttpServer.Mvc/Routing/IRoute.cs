using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Mvc.Routing
{
    /// <summary>
    /// Route reques to a controller.
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// Route the HTTP request
        /// </summary>
        /// <param name="context">Context information</param>
        /// <returns>Type if controller was found; otherwise null.</returns>
        RouteResult RouteRequest(RoutingContext context);
    }
}
