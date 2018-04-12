using System;
using System.Collections.Generic;
using System.Reflection;
using HttpServer.Logging;
using HttpServer.Mvc.Controllers;

namespace HttpServer.Mvc.Routing
{
    public class Route : IRoute
    {
        private readonly ILogger _logger = LogFactory.CreateLogger(typeof (RoutingService));

        private readonly Dictionary<string, MethodMapping> _methods =
            new Dictionary<string, MethodMapping>(StringComparer.OrdinalIgnoreCase);

        private MethodInfo _defaultMethod;

        public string ControllerUri { get; set; }

        public string ControllerName { get; set; }

        public int UriSegments { get; set; }

        public Type ControllerType { get; set; }

        #region IRoute Members

        public RouteResult RouteRequest(RoutingContext context)
        {
            // Uri adds one segment extra (counts slashes)
            var requestUriSegments = context.Request.Uri.Segments.Length - 1;

            if (!context.Request.Uri.AbsolutePath.StartsWith(ControllerUri))
                return null;


            if (requestUriSegments < UriSegments)
                return null;

            string actionUri = ControllerUri;
            string actionName;
            MethodInfo method;
            // using default action
            if (requestUriSegments == UriSegments)
            {
                if (_defaultMethod == null)
                    return null;

                method = _defaultMethod;
                actionName = _defaultMethod.Name;
            }
            else
            {
                actionName = context.Request.Uri.Segments[UriSegments + 1].TrimEnd('/');
                method = GetMethod(context.Request.Method, actionName);
                if (method == null)
                    return null;
            }

            return new RouteResult
                       {
                           ControllerType = ControllerType,
                           Action = method,
                           ActionName = actionName,
                           ControllerUri = ControllerUri
                       };
        }

        #endregion

        private MethodInfo GetMethod(string httpMethod, string actionName)
        {
            MethodMapping mapping;
            if (!_methods.TryGetValue(actionName, out mapping))
                return null;
            var method = mapping.Get(httpMethod);
            return method;
        }

        public void Map()
        {
            var actionResultType = typeof (IActionResult);
            var viewDataType = typeof (IViewData);

            MethodInfo indexMethod = null;
            foreach (var methodInfo in ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (methodInfo.IsAbstract)
                    continue;
                if (methodInfo.Name.StartsWith("get_") || methodInfo.Name.StartsWith("set_"))
                    continue;
                if (methodInfo.DeclaringType == typeof (Controller))
                    continue;
                if (methodInfo.GetParameters().Length > 0)
                    continue;

                if (methodInfo.ReturnType == actionResultType)
                    MapMethod(methodInfo);
                if (methodInfo.ReturnType == viewDataType)
                    MapMethod(methodInfo);

                if (methodInfo.Name.ToLower() == "index" && methodInfo.GetParameters().Length == 0 &&
                    (methodInfo.ReturnType == actionResultType || methodInfo.ReturnType == viewDataType))
                    indexMethod = methodInfo;

                CheckIfDefaultMethod(methodInfo);
            }


            // use "Index" method if no other default method have been specified.
            if (_defaultMethod == null && indexMethod != null)
                _defaultMethod = indexMethod;
        }


        private MethodContext TryGetVerb(MethodInfo info)
        {
            var ctx = new MethodContext {ActionName = info.Name.ToLower()};


            foreach (var httpMethod in Method.Methods)
            {
                if (!ctx.ActionName.EndsWith(httpMethod.ToLower())) continue;
                ctx.HttpMethod = httpMethod;
                ctx.ActionName = ctx.ActionName.Remove(ctx.ActionName.Length - httpMethod.Length);
                return ctx;
            }

            var attributes = info.GetCustomAttributes(typeof (ValidForAttribute), true);
            if (attributes.Length == 1)
            {
                ctx.HttpMethod = ((ValidForAttribute) attributes[0]).Method;
                return ctx;
            }

            return null;
        }

        private void MapMethod(MethodInfo info)
        {
            var ctx = TryGetVerb(info) ?? new MethodContext {ActionName = info.Name.ToLower()};


            MethodMapping mapping;
            if (!_methods.TryGetValue(ctx.ActionName.ToLower(), out mapping))
            {
                mapping = new MethodMapping();
                _methods.Add(ctx.ActionName, mapping);
            }

            // specified a verb
            if (!string.IsNullOrEmpty(ctx.HttpMethod))
            {
                _logger.Trace("Mapped " + ControllerUri + "." + ctx.ActionName + " to " + ctx.HttpMethod);
                mapping.IsMethodsSpecified = true;
                mapping.Add(ctx.HttpMethod, info);
            }


            if (!mapping.IsMethodsSpecified)
            {
                _logger.Trace("Mapped " + ControllerUri + "." + ctx.ActionName + " to ALL verbs");
                mapping.Add(Method.Unknown, info);
            }
        }

        private void CheckIfDefaultMethod(MethodInfo methodInfo)
        {
            foreach (var customAttribute in methodInfo.GetCustomAttributes(false))
            {
                if (!(customAttribute is DefaultActionAttribute)) continue;

                if (_defaultMethod != null)
                {
                    _logger.Warning("Tried to map '" + methodInfo.Name + "' in '" + ControllerType.FullName +
                                    "' as default method when '" + _defaultMethod.Name +
                                    "' have already been specified.");

                    throw new ActionMappingException("Default method have already been mapped to '" +
                                                     _defaultMethod.Name + "' in controller '" + ControllerType.FullName +
                                                     "'");
                }

                //var attribute = (DefaultActionAttribute) customAttribute;
                _logger.Trace("Mapping default method '" + methodInfo.Name + "'.");
                _defaultMethod = methodInfo;
            }
        }

        #region Nested type: MethodContext

        private class MethodContext
        {
            public string ActionName { get; set; }
            public string HttpMethod { get; set; }
        }

        #endregion
    }
}