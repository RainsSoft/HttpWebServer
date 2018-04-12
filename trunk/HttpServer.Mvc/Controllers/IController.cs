using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Mvc.Controllers
{
    public interface IController
    {
        void OnActionExecuting(ActionExecutingContext context);
        void OnActionExecuted(ActionExecutedContext context);
        void OnException(ExceptionContext context);
    }

    public class ActionExecutingContext
    {
        
    }

    public class ActionExecutedContext
    {

    }

    public class ExceptionContext
    {
        public Exception Exception { get; private set; }
        public IActionResult Result { get; set; }

        public ExceptionContext(Exception exception)
        {
            Exception = exception;
        }
    }
}
