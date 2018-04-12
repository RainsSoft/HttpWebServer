using System;
using System.Collections.Generic;
using HttpServer.Logging;

namespace HttpServer.Mvc.Routing
{
    public class RoutingService
    {
        private readonly ILogger _logger = LogFactory.CreateLogger(typeof (RoutingService));
        private readonly List<Route> _routes = new List<Route>();


        /// <summary>
        /// Find controller name (that are used when routing requests)
        /// </summary>
        /// <param name="type">Controller type</param>
        public void RegisterController(Type type)
        {
            Route route = CreateBaseRoute(type);
            route.Map();
            _routes.Add(route);
        }

        public string GetUriFor(Type controllerType)
        {
            return CreateBaseRoute(controllerType).ControllerUri;
        }

        private Route CreateBaseRoute(Type type)
        {
            object[] attributes = type.GetCustomAttributes(false);
            string uri = null;
            string controllerName = "";

            foreach (object attribute in attributes)
            {
                var controllerNameAttribute = attribute as ControllerNameAttribute;
                if (controllerNameAttribute != null)
                {
                    uri = "/" + controllerNameAttribute.Name + "/";
                    controllerName = controllerNameAttribute.Name;
                }
                var attr = attribute as ControllerUriAttribute;
                if (attr != null)
                {
                    controllerName = attr.Uri.TrimStart('/').TrimEnd('/');
                    uri = attr.Uri;
                    if (!uri.EndsWith("/"))
                        uri += "/";
                    if (!uri.StartsWith("/"))
                        uri = "/" + uri;
                }
            }


            // Name has not been speecified, use controller type name.
            if (string.IsNullOrEmpty(uri))
            {
                controllerName = type.Name.EndsWith("Controller")
                                     ? type.Name.Replace("Controller", string.Empty)
                                     : type.Name;
                uri = "/" + controllerName + "/";
            }

            controllerName = controllerName.ToLower();
            uri = uri.ToLower();
            int uriSegments = 0;
            foreach (char ch in uri)
            {
                if (ch == '/')
                    ++uriSegments;
            }
            if (uri.EndsWith("/"))
                --uriSegments; // else we get one part to many

            _logger.Debug("Added controller '" + uri + "'");

            var route = new Route
                            {
                                ControllerUri = uri,
                                ControllerName = controllerName,
                                UriSegments = uriSegments,
                                ControllerType = type
                            };

            return route;
        }

        public RouteResult Route(RoutingContext routingContext)
        {
            foreach (Route route in _routes)
            {
                RouteResult result = route.RouteRequest(routingContext);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}