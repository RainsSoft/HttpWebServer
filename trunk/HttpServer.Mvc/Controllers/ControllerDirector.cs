using System;
using System.Collections.Generic;
using System.Reflection;
using HttpServer.Logging;

namespace HttpServer.Mvc.Controllers
{
    /// <summary>
    /// Takes care of all controller routing
    /// </summary>
    /// <remarks>
    /// Default implementation. Handles all controllers derived from <see cref="Controller"/>.
    /// </remarks>
    [Obsolete("Look at the IController interface and the ControllerFactory")]
    public class ControllerDirector : IControllerDirector
    {
        private readonly Controller _controllerPrototype;
        private readonly Queue<Controller> _createdControllers = new Queue<Controller>();
        private readonly ILogger _logger = LogFactory.CreateLogger(typeof (ControllerDirector));
        private readonly Dictionary<string, MethodMapping> _methods =
            new Dictionary<string, MethodMapping>(StringComparer.OrdinalIgnoreCase);
        private readonly Type _type;
        private MethodInfo _defaultMethod; // method to invoke if no action has been specified.
        private string _uri; // uri to come to the controller
        private int _uriSegments; // number of uri parts
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDirector"/> class.
        /// </summary>
        /// <param name="controller">Controller to handle.</param>
        public ControllerDirector(Controller controller)
        {
            _controllerPrototype = controller;
            _type = controller.GetType();
            FindControllerName(_type);
            //controller.ControllerUri = _uri;
            //controller.ControllerName = _name;

            Type actionResultType = typeof (IActionResult);
            Type viewDataType = typeof (IViewData);

            MethodInfo indexMethod = null;
            foreach (MethodInfo methodInfo in _type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
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

        /// <summary>
        /// Gets controller uri
        /// </summary>
        public string Uri
        {
            get { return _uri; }
        }

        /// <summary>
        /// Determines if this controller director can process the specified context.
        /// </summary>
        /// <param name="context">Context being processed.</param>
        /// <returns><c>true</c> if this director should process the context; otherwise <c>false</c>.</returns>
        public bool CanProcess(ControllerContext context)
        {
            if (!context.Uri.AbsolutePath.StartsWith(_uri))
                return false;

            if (context.UriSegments.Length < _uriSegments)
                return false;

            // using default action
            if (context.UriSegments.Length == _uriSegments)
            {
                if (_defaultMethod == null)
                    return false;
            }
            else
            {
                string actionName = context.UriSegments[_uriSegments];
                MethodInfo method = GetMethod(context.RequestContext.Request.Method, actionName);
                if (method == null)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if a method is tagged with default attribute.
        /// </summary>
        /// <param name="methodInfo">Method to check</param>
        /// <exception cref="ActionMappingException">Default method have already been specified.</exception>
        private void CheckIfDefaultMethod(MethodInfo methodInfo)
        {
            foreach (object customAttribute in methodInfo.GetCustomAttributes(false))
            {
                if (!(customAttribute is DefaultActionAttribute)) continue;

                if (_defaultMethod != null)
                {
                    _logger.Warning("Tried to map '" + methodInfo.Name + "' in '" + _type.FullName +
                                    "' as default method when '" + _defaultMethod.Name +
                                    "' have already been specified.");

                    throw new ActionMappingException("Default method have already been mapped to '" +
                                                     _defaultMethod.Name + "' in controller '" + _type.FullName +
                                                     "'");
                }

                //var attribute = (DefaultActionAttribute) customAttribute;
                _logger.Trace("Mapping default method '" + methodInfo.Name + "'.");
                _defaultMethod = methodInfo;
            }
        }

        /// <summary>
        /// Get a cached controller or create a new one.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// We need to have one controller per simultaneous request since
        /// all properties used in the controller will otherwise get overwritten
        /// each time a new request is invoked.
        /// </remarks>
        private Controller CreateController()
        {
            Controller controller = null;
            lock (_createdControllers)
            {
                if (_createdControllers.Count > 0)
                    controller = _createdControllers.Dequeue();
            }

            if (controller == null)
            {
                controller = (Controller)_controllerPrototype.Clone();
                //controller.ControllerUri = _uri;
                //controller.ControllerName = _name;
            }

            controller.Clear();
            return controller;
        }

        /// <summary>
        /// Find controller name (that are used when routing requests)
        /// </summary>
        /// <param name="type">Controller type</param>
        private void FindControllerName(Type type)
        {
            _uri = null;
            object[] attributes = type.GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is ControllerNameAttribute)
                {
                    var controllerNameAttribute = (ControllerNameAttribute)attribute;
                    _uri = "/" + controllerNameAttribute.Name + "/";
                    _name = controllerNameAttribute.Name;
                }
                if (attribute is ControllerUriAttribute)
                {
                    var attr = (ControllerUriAttribute) attribute;
                    _name = attr.Uri.TrimStart('/').TrimEnd('/');
                    _uri = attr.Uri;
                    if (!_uri.EndsWith("/"))
                        _uri += "/";
                    if (!_uri.StartsWith("/"))
                        _uri = "/" + _uri;
                }
            }



            // Name has not been speecified, use controller type name.
            if (string.IsNullOrEmpty(_uri))
            {
                _name = type.Name.EndsWith("Controller")
                           ? type.Name.Replace("Controller", string.Empty)
                           : type.Name;
                _uri = "/" + _name + "/";

            }

            _name = _name.ToLower();
            _uri = _uri.ToLower();
            _uriSegments = 0;
            foreach (char ch in _uri)
            {
                if (ch == '/')
                    ++_uriSegments;
            }
            if (_uri.EndsWith("/"))
                --_uriSegments; // else we get one part to many

            _logger.Debug("Added controller '" + _uri + "'");
        }

        private MethodInfo GetMethod(string httpMethod, string actionName)
        {
            MethodMapping mapping;
            if (!_methods.TryGetValue(actionName, out mapping))
                return null;
            MethodInfo method = mapping.Get(httpMethod);
            return method;
        }


        /// <summary>
        /// Trigger the InvokingAction event.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        /// <param name="context"></param>
        /// <remarks>
        /// <para>
        /// We need to capture all exceptions since we have no control
        /// over what the event subscribers do, and we do not want the 
        /// whole server to crash because of an unhandled exception.
        /// </para>
        /// <para>
        /// HttpExceptions are a different story since they are handled
        /// in a lower level in the framework.
        /// </para>
        /// </remarks>
        /// <exception cref="Exception">A HTTP exception has been thrown by an event subscriber.</exception>
        private void InvokeEvent(Controller controller, MethodInfo method, RequestContext context)
        {
            try
            {
                InvokingAction(this, new ControllerEventArgs(controller, method.Name.ToLower(), context));
            }
            catch (Exception err)
            {
                if (err is HttpException)
                    throw;
                _logger.Error("Event threw exception: " + err);
            }
        }

        /// <summary>
        /// Map methods to actions.
        /// </summary>
        /// <param name="info">Method to inspect.</param>
        /// <remarks>Goes through all custom action attributes to check which HTTP methods
        /// the action is valid for.</remarks>
        /// <exception cref="ActionMappingException">Mapping collision. Do not both suffix methods with a verb and use ValidFor attribute.</exception>
        private void MapMethod(MethodInfo info)
        {
            string verb;
            string methodName = info.Name;
            bool isFound = TryGetVerb(ref methodName, out verb);

            MethodMapping mapping;
            if (!_methods.TryGetValue(methodName.ToLower(), out mapping))
            {
                mapping = new MethodMapping();
                _methods.Add(methodName.ToLower(), mapping);
            }

            // specified a verb
            if (isFound)
            {
                _logger.Trace("Mapped " + _uri + "." + methodName.ToLower() + " to " + verb);
                mapping.IsMethodsSpecified = true;
                mapping.Add(verb, info);
            }


            // Check if method is valid only for a few verbs
            foreach (object customAttribute in info.GetCustomAttributes(false))
            {
                var attr = customAttribute as ValidForAttribute;
                if (attr == null) continue;
                if (isFound)
                    throw new ActionMappingException(
                        "Mapping collision. Do not both suffix methods with a verb and use ValidFor attribute.");

                mapping.IsMethodsSpecified = true;
                
            }

            if (!mapping.IsMethodsSpecified)
            {
                _logger.Trace("Mapped " + _uri + "." + methodName.ToLower() + " to ALL verbs");
                mapping.Add(verb, info);
            }
        }

        /// <summary>
        /// Get handler for a specific verb.
        /// </summary>
        /// <param name="name">Method name (should be prefixed with verb)</param>
        /// <param name="verb">Verb to get</param>
        /// <returns>Method.Unknown if verb could not be found.</returns>
        /// <remarks>
        /// Since we can map different HTTP verbs to different methods,
        /// we need to check if only one or multiple methods have been mapped
        /// to an action.
        /// </remarks>
        /// <example>
        /// <code>
        /// string name = "ViewPost";
        /// Method method;
        /// director.TryGetVerb(ref name, out method);
        /// </code>
        /// </example>
        private bool TryGetVerb(ref string name, out string verb)
        {
            name = name.ToLower();
            foreach (string httpMethod in Method.Methods)
            {
                if (!name.EndsWith(httpMethod.ToLower())) continue;
                verb = httpMethod;
                name = name.Remove(name.Length - httpMethod.Length);
                return true;
            }

            verb = Method.Unknown;
            return false;
        }

        #region IControllerDirector Members

        /// <summary>
        /// Raised before a controller action is invoked.
        /// </summary>
        /// <remarks>Use it to invoke any controller initializations you might need to do.</remarks>
        public event EventHandler<ControllerEventArgs> InvokingAction = delegate { };

        /// <summary>
        /// Get all routings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetRoutes()
        {
            var routes = new List<string>();
            foreach (var actionName in _methods.Keys)
            {
                routes.Add(_uri + actionName);
            }
            return routes;
        }

        /// <summary>
        /// Process controller request.
        /// </summary>
        /// <param name="context">Controller context</param>
        /// <param name="controller">Controller used to process request</param>
        /// <returns>Action result.</returns>
        /// <exception cref="NotFoundException"><c>NotFoundException</c>.</exception>
        /// <exception cref="InvalidOperationException">Specified URI is not for this controller.</exception>
        /// <remarks>
        /// Controller is returned as a parameter to let us be able to use it as a variable
        /// in the view. It's VERY important <see cref="Enqueue"/> is called when rendering is complete (or if an exception
        /// is thrown).
        /// </remarks>
        public object Process(IControllerContext context, out Controller controller)
        {
            if (!context.Uri.AbsolutePath.StartsWith(_uri))
                throw new InvalidOperationException("Uri '" + context.Uri + "' is not for controller '" + _type.FullName +
                                                    "'.");
            controller = null;
            MethodInfo method;

            if (context.UriSegments.Length < _uriSegments)
                throw new NotFoundException("No action specified for controller '" + _uri + "'.");

            // using default action
            if (context.UriSegments.Length == _uriSegments)
            {
                if (_defaultMethod == null)
                    return null;
                method = _defaultMethod;
            }
            else
            {
                string actionName = context.UriSegments[_uriSegments];
                method = GetMethod(context.RequestContext.Request.Method, actionName);
                if (method == null)
                    return null;
            }

            context.ControllerUri = _uri;
            context.ControllerName = _name;
            context.ActionName = context.ActionName = method.Name.ToLower();

            // invoke method
            controller = CreateController();
            try
            {
                controller.Clear();
                InvokeEvent(controller, method, context.RequestContext);
                controller.SetContext(context);

                // Before action can filter out stuff.
                var actionResult = controller.InvokeBeforeAction(method);
                if (actionResult != null)
                    return actionResult;

                var result = method.Invoke(controller, null);
                context.Title = controller.Title;
                context.LayoutName = controller.LayoutName;
                controller.InvokeAfterAction((IActionResult)result);
                return result;
            }
            catch (Exception err)
            {
                var result = controller.TriggerOnException(err);
                if (result == null)
                {
                    Enqueue(controller);
                    throw;
                }

                controller.InvokeAfterAction(result);
                return result;
            }
        }

        /// <summary>
        /// Enqueue a used controller.
        /// </summary>
        /// <param name="controller">Controller to enqueue</param>
        public void Enqueue(Controller controller)
        {
                lock (_createdControllers)
                    _createdControllers.Enqueue(controller);
        }

        #endregion

        #region Nested type: MethodMapping

        private class MethodMapping
        {
            private readonly Dictionary<string, MethodInfo> _mappings = new Dictionary<string, MethodInfo>();
            private MethodInfo _default;

            /// <summary>
            /// Gets or sets if HTTP methods are specified.
            /// </summary>
            public bool IsMethodsSpecified { get; set; }

            public void Add(string method, MethodInfo info)
            {
                // Second call means that the user have
                // mapped a specific method.
                // copy the previous default one to 
                // get.
                if (_default != null)
                    _mappings[Method.Get] = _default;
                else
                    _default = info;

                _mappings.Add(method, info);
            }

            public MethodInfo Get(string method)
            {
                if (!IsMethodsSpecified)
                    return _default;

                MethodInfo mi;
                return _mappings.TryGetValue(method, out mi) ? mi : null;
            }
        }

        #endregion
    }
}