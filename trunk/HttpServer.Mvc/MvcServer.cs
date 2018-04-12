using System;
using System.Collections.Generic;
using System.IO;
using HttpServer.Logging;
using HttpServer.Modules;
using HttpServer.Mvc.ActionResults;
using HttpServer.Mvc.Controllers;
using HttpServer.Mvc.Routing;
using HttpServer.Mvc.Views;
using HttpServer.Resources;

namespace HttpServer.Mvc
{
    /// <summary>
    /// MVC web server.
    /// </summary>
    public class MvcServer : Server, IModule
    {
        [ThreadStatic]
        private static MvcServer _current;
        private readonly ActionResultProvider _actionProvider = new ActionResultProvider();
        private readonly BuiltinActions _actions = new BuiltinActions();
        private readonly List<ControllerDirector> _directors = new List<ControllerDirector>();
        private readonly ILogger _logger = LogFactory.CreateLogger(typeof(MvcServer));
        private readonly ViewEngineCollection _viewEngines;
        private readonly ResourceProvider _viewProvider = new ResourceProvider();
        private Dictionary<string, ControllerDirector> _routes = new Dictionary<string, ControllerDirector>();
        ControllerFactory _factory = new ControllerFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcServer"/> class.
        /// </summary>
        public MvcServer()
        {
            _current = this;
            Add(this);
            _viewEngines = new ViewEngineCollection();

            // register built in actions
            _actions.Register(_actionProvider);

            RoutingService = new RoutingService();
        }

        /// <summary>
        /// Gets all action handlers.
        /// </summary>
        public ActionResultProvider ActionProvider
        {
            get { return _actionProvider; }
        }

        public RoutingService RoutingService { get; set; }
        /// <summary>
        /// Gets current web server
        /// </summary>
        public static MvcServer CurrentMvc
        {
            get { return _current; }
        }

        /// <summary>
        /// Gets view engine collection.
        /// </summary>
        public ViewEngineCollection ViewEngines
        {
            get { return _viewEngines; }
        }

        /// <summary>
        /// Gets view provider.
        /// </summary>
        public ResourceProvider ViewProvider
        {
            get { return _viewProvider; }
        }

        public ControllerFactory ControllerFactory
        {
            get {
                return _factory;
            }
        }

        /// <summary>
        /// Add a controller.
        /// </summary>
        /// <param name="controller"></param>
        [Obsolete("Use ControllerFactory.Current.Register() instead.")]
        public void Add(Controller controller)
        {
            _factory.Register(controller.GetType());
            RoutingService.RegisterController(controller.GetType());
        }

        private void OnInvokingAction(object sender, ControllerEventArgs e)
        {
            InvokingAction(this, e);
        }

        private void RenderView(ControllerContext controllerContext, IViewData viewData)
        {
            //ViewProvider.Get(controllerContext.ViewPath + ".*");


            // do not dispose writer, since it will close the stream.
            TextWriter bodyWriter = new StreamWriter(controllerContext.RequestContext.Response.Body);


            _viewEngines.Render(bodyWriter, controllerContext, viewData);
            bodyWriter.Flush();
        }

        #region IModule Members

        /// <summary>
        /// Process a request.
        /// </summary>
        /// <param name="context">Request information</param>
        /// <returns>What to do next.</returns>
        public ProcessingResult Process(RequestContext context)
        {
            _current = this;
            var routingContext = new RoutingContext(context.Request);
            var routeResult = RoutingService.Route(routingContext);
            if (routeResult == null)
                return ProcessingResult.Continue;

            var controllerContext = new ControllerContext(context)
                                        {ActionName = routeResult.ActionName, ControllerUri = routeResult.ControllerUri};
            var result = _factory.Invoke(routeResult.ControllerType, routeResult.Action, controllerContext);

            var viewData = result as IViewData;
            if (viewData != null)
            {
                _logger.Trace("Rendering action " + controllerContext.ActionName);
                RenderView(controllerContext, viewData);
            }
            else
            {
                var action = result as IActionResult;
                if (action != null)
                {
                    ProcessingResult processingResult = _actionProvider.Invoke(context, action);
                    if (processingResult == ProcessingResult.Abort)
                        return processingResult;
                }
            }

            return ProcessingResult.SendResponse;

        }

        #endregion

        /// <summary>
        /// Raised before a controller action is invoked.
        /// </summary>
        /// <remarks>Use it to invoke any controller initializations you might need to do.</remarks>
        public event EventHandler<ControllerEventArgs> InvokingAction = delegate { };

        public void Register(Type controllerType)
        {
            ControllerFactory.Register(controllerType);
            RoutingService.RegisterController(controllerType);
        }
    }
}