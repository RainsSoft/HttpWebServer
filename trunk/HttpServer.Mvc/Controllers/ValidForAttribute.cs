using System;

namespace HttpServer.Mvc.Controllers
{
    /// <summary>
    /// Action is valid for specific HTTP methods.
    /// </summary>
    public class ValidForAttribute : Attribute
    {
        public ValidForAttribute(string method)
        {
            if (method == null) throw new ArgumentNullException("method");
            Method = method;
        }

        /// <summary>
        /// Methods that this action is valid for.
        /// </summary>
        public string Method { get; private set; }
    }
}