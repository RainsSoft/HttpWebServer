using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HttpServer.MVC2.ActionResults;

namespace HttpServer.MVC2.Controllers
{
    /// <summary>
    /// Contains uri to action method mappings for a controller
    /// </summary>
    /// <remarks>
    /// Mappings are made on action name in combination with argument count. This means that
    /// the same action can have multiple methods as long as they got different number of arguments.
    /// </remarks>
    public class ControllerMapping
    {
        /// <summary>
        /// Key is "action|argumentCount".
        /// </summary>
        private List<MethodBase> _actions = new List<MethodBase>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerMapping"/> class.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <param name="uri">Uri to controller.</param>
        public ControllerMapping(Type controllerType, string uri)
        {
            ControllerType = controllerType;
            Uri = uri;
        }

        /// <summary>
        /// Gets or sets URI to reach this route.
        /// </summary>
        public string Uri { get; set; }


        /// <summary>
        /// Gets or sets type of controller to invoke
        /// </summary>
        public Type ControllerType { get; set; }

        public IEnumerable<string> Mappings
        {
            get
            {
                List<string> mappings = new List<string>();
                foreach (var action in _actions)
                {
                    if (!mappings.Contains(action.Name))
                        mappings.Add(action.Name);
                }
                return mappings;
            }
        }

        /// <summary>
        /// Invoke an action method.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action.</param>
        /// <param name="arguments">Action arguments.</param>
        /// <returns></returns>
        internal IActionResult Invoke(IController instance, string action, object[] arguments)
        {
            return  (IActionResult)_actions[action].Invoke(instance, arguments);
        }

        /// <summary>
        /// Add a action method.
        /// </summary>
        /// <param name="method">The method.</param>
        public void Add(MethodInfo method)
        {
            _actions.Add(method.Name + "|" + method.GetParameters().Length, method);
        }

        public IEnumerable<MethodBase> FindAction(string actionName)
        {
            return _actions.Where(p => p.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
