using System;
using System.Collections.Generic;
using System.Text;
using HttpServer.Logging;
using HttpServer.Mvc.Routing;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Implement the IServiceResolver interface to get support for IoC.
    /// </summary>
    public class ServiceResolver
    {
        private static ServiceResolver _instance = new ServiceResolver(new SimpleFactoryResolver());
        private IServiceResolver _resolver;

        private ServiceResolver(IServiceResolver serviceResolver)
        {
            _resolver = serviceResolver;
        }

        /// <summary>
        /// Gets current implementation
        /// </summary>
        public static ServiceResolver Current
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("You must implement and assign service resolver.");
                return _instance;
            }
        }

        /// <summary>
        /// Assign your own implementation
        /// </summary>
        /// <param name="factory">Factory to use</param>
        public void Assign(IServiceResolver factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            _instance = new ServiceResolver(factory);
        }

        public virtual T Resolve<T>() where T : class
        {
            return (T)_resolver.Resolve(typeof(T));
        }

        public virtual object Resolve(Type type) 
        {
            return _resolver.Resolve(type);
        }
    }
}
