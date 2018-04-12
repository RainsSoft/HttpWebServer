using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.Modules;

namespace HttpServer.MVC2.Controllers
{
    class ControllerModule : IModule
    {
        ControllerFactory _controllerFactory = new ControllerFactory();

        /// <summary>
        /// Key is complete uri to action
        /// </summary>
        private Dictionary<string, ControllerMapping> _controllers =
            new Dictionary<string, ControllerMapping>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Process a request.
        /// </summary>
        /// <param name="context">Request information</param>
        /// <returns>What to do next.</returns>
        public ProcessingResult Process(RequestContext context)
        {
            var uri = context.Request.Uri.AbsolutePath.TrimEnd('/');

            ControllerMapping mapping;
            if (!_controllers.TryGetValue(uri, out mapping))
                return ProcessingResult.Continue;

            int pos = uri.LastIndexOf('/');
            var actionName = uri.Substring(pos + 1);

            var controller = _controllerFactory.Create(mapping.ControllerType);
            var actions = mapping.FindAction(actionName);

            var form = context.Request.Form;
            var queryString = context.Request.QueryString;
            foreach (var action in actions)
            {
                

                foreach (var parameter in action.GetParameters())
                {
                    var actionParameter = parameter;
                    var httpParameter =
                        form.FirstOrDefault(
                            p => p.Name.Equals(actionParameter.Name, StringComparison.OrdinalIgnoreCase)) ??
                        queryString.FirstOrDefault(
                            p => p.Name.Equals(actionParameter.Name, StringComparison.OrdinalIgnoreCase));

                }
            }



            var result = mapping.Invoke(controller, actionName, null);
            return ProcessingResult.SendResponse;
        }

        /// <summary>
        /// Loads Uri mappings for all controllers and their actions
        /// </summary>
        public void Load()
        {
            ControllerIndexer indexer = new ControllerIndexer();
            indexer.Find();
            foreach (var controller in indexer.Controllers)
                foreach (var mapping in controller.Mappings)
                    _controllers.Add(controller.Uri + "/" + mapping, controller);
        }
    }
}
