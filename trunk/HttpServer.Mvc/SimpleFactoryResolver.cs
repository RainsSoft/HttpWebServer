using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Use this one to get the old functionality.
    /// </summary>
    public class SimpleFactoryResolver : IServiceResolver
    {
        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
