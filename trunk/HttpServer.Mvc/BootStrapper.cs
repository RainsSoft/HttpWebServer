using System;
using System.Reflection;
using HttpServer.Mvc.Controllers;
using HttpServer.Resources;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Helper to make it easier to setup a MVC web server.
    /// </summary>
    public class BootStrapper
    {
        private readonly MvcServer _server;
        private EmbeddedResourceLoader _resource = new EmbeddedResourceLoader();

        /// <summary>
        /// Initializes a new instance of the <see cref="BootStrapper"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        public BootStrapper(MvcServer server)
        {
            _server = server;
            _server.ViewProvider.Add(_resource);
        }

        /// <summary>
        /// Add all controllers found in the specified assemblies.
        /// </summary>
        /// <param name="assemblies">Assemblies to search</param>
        public void LoadControllers(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
                LoadControllers(assembly);
        }

        /// <summary>
        /// Add all controllers found in the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <remarks>It will also look for views in a subfolder to the controller.</remarks>
        public void LoadControllers(Assembly assembly)
        {
            Type controllerType = typeof(Controller);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!controllerType.IsAssignableFrom(type) && type != controllerType)
                    continue;

                // find default constructor
                bool found = false;
                foreach (ConstructorInfo info in type.GetConstructors())
                {
                    if (info.GetParameters().Length != 0) continue;
                    found = true;
                    break;
                }
                if (!found)
                    continue;

                _server.Register(type);
                var uri = _server.RoutingService.GetUriFor(type);
                _resource.AddFilesInFolder(uri, type.Assembly, type.Namespace + ".Views");
            }            
            
        }

        /// <summary>
        /// Add views embedded in the assembly.
        /// </summary>
        /// <param name="assembly">Assembly that the views are located in.</param>
        /// <remarks>
        /// Will look after views that has the name "*.Views.[Controller].[ActioName].*",
        /// "*.[Controller].Views.[ActionName].*" and "*.Shared.[LayoutName].*"
        /// </remarks>
        public void LoadEmbeddedViews(Assembly assembly)
        {
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                string lowerName = resourceName.ToLower();
                if (!lowerName.Contains("views") && !lowerName.Contains("shared"))
                    continue;

                string[] parts = lowerName.Split('.');
                if (parts.Length < 3)
                    continue;

                string extension = parts[parts.Length - 1];
                if (extension == "cs" || extension == "vb" || extension == "res")
                    continue;

                string actionName = parts[parts.Length - 2];
                string controller = parts[parts.Length - 3];

                // Got a layout.
                if (controller == "shared")
                {
                    _resource.AddFile(controller + "/" + actionName + "." + extension, assembly, resourceName);
                    continue;
                }

                // must contain controllerName, views, actionname, extension
                // in one of the specified orders.
                if (parts.Length < 4)
                    continue;

                if (controller == "views")
                    controller = parts[parts.Length - 4];
                else if (parts[parts.Length-4] != "views")
                    continue; // must be Views.[ControllerName].[ActionName].*

                _resource.AddFile(controller + "/" + actionName + "." + extension, assembly, resourceName);
            }
        }
    }
}
