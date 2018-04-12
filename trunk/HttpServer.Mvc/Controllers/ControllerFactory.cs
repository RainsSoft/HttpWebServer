using System;
using System.Collections.Generic;
using System.Reflection;

namespace HttpServer.Mvc.Controllers
{
    public class ControllerFactory
    {
        private readonly List<Type> _controllers =
            new List<Type>();

        public void Register<T>() where T : Controller
        {
            var controllerType = typeof (T);
            Register(controllerType);
        }

        public virtual void Register(Type controllerType)
        {
            if (controllerType == null) throw new ArgumentNullException("controllerType");
            if (!typeof (Controller).IsAssignableFrom(controllerType))
                throw new InvalidOperationException(string.Format("'{0}' is not a controller", controllerType.FullName));

            _controllers.Add(controllerType);
        }

        public virtual object Invoke(Type controllerType, MethodInfo action, IControllerContext context)
        {
            var controller = (Controller) ServiceResolver.Current.Resolve(controllerType);
            var newController = controller as IController;
            try
            {
                controller.Clear();
                controller.SetContext(context);


                if (newController != null)
                {
                    var ctx = new ActionExecutingContext();
                    newController.OnActionExecuting(ctx);
                }

                var result = action.Invoke(controller, null);
                context.Title = controller.Title;
                context.LayoutName = controller.LayoutName;

                if (newController != null)
                {
                    var ctx = new ActionExecutedContext();
                    newController.OnActionExecuted(ctx);
                }

                return result;
            }
            catch (Exception err)
            {
                if (newController != null)
                {
                    var ctx = new ExceptionContext(err);
                    newController.OnException(ctx);
                    if (ctx.Result != null)
                        return ctx.Result;
                }

                var result = controller.TriggerOnException(err);
                if (result != null)
                    return result;

                throw;
            }
        }
    }

}