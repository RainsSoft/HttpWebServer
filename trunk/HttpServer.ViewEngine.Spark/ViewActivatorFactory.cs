using System;
using Spark;

namespace HttpServer.ViewEngine.Spark
{
    internal class ViewActivatorFactory : IViewActivatorFactory
    {
        #region IViewActivatorFactory Members

        public IViewActivator Register(Type type)
        {
            throw new NotImplementedException();
        }

        public void Unregister(Type type, IViewActivator activator)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}