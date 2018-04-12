using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HttpServer.MVC2.ActionResults;

namespace HttpServer.MVC2.Controllers
{
    public class ControllerIndexer
    {
        private List<ControllerMapping> _controllers = new List<ControllerMapping>();

        public IEnumerable<ControllerMapping> Controllers
        {
            get { return _controllers; }
        }

        public void Find()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Find(assembly);
            }
        }

        public void Find(Assembly assembly)
        {
            var controllerType = typeof (IController);
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!controllerType.IsAssignableFrom(type))
                    continue;

                MapController(type);
            }
        }

        private void MapController(Type type)
        {
            string uri = type.Name.Replace("Controller", string.Empty);
            var attributes = type.GetCustomAttributes(typeof (ControllerUriAttribute), false);
            if (attributes.Length > 0)
                uri = ((ControllerUriAttribute) attributes[0]).Uri;

            ControllerMapping mapping = new ControllerMapping(type, uri);
            var resultType = typeof (IActionResult);
            foreach (var method in type.GetMethods())
            {
                if (!resultType.IsAssignableFrom(method.ReturnType))
                    continue;

                mapping.Add(method);
            }

            _controllers.Add(mapping);
        }
    }
}
