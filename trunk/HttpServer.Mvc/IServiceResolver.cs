using System;

namespace HttpServer.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceResolver
    {
        object Resolve(Type type);
    }
}