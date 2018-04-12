using System;
using System.Collections.Generic;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Provides action results to 
    /// </summary>
    public class ActionResultProvider
    {
        private readonly Dictionary<Type, ActionHandler> _actions = new Dictionary<Type, ActionHandler>();

        /// <summary>
        /// Register a new action processor.
        /// </summary>
        /// <typeparam name="T">Type of action to handle.</typeparam>
        /// <param name="handler">Delegate processing the action</param>
        public void Register<T>(ActionHandler handler) where T : IActionResult
        {
            _actions.Add(typeof (T), handler);
        }

        /// <summary>
        /// Invoke action
        /// </summary>
        /// <param name="context">Action context</param>
        /// <param name="action">Action to invoke.</param>
        /// <exception cref="NotSupportedException">No registered handler for the specified action type.</exception>
        internal ProcessingResult Invoke(RequestContext context, IActionResult action)
        {
            ActionHandler handler;
            if (!_actions.TryGetValue(action.GetType(), out handler))
                throw new NotSupportedException("No handler for action type '" + action.GetType().FullName + "'.");

            return handler(context, action);
        }

    }

    /// <summary>
    /// Used by <see cref="ActionResultProvider"/>
    /// </summary>
    /// <param name="context">Request context</param>
    /// <param name="action">Action to process</param>
    public delegate ProcessingResult ActionHandler(RequestContext context, IActionResult action);
}
