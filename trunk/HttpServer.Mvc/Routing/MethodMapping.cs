using System.Collections.Generic;
using System.Reflection;

namespace HttpServer.Mvc.Routing
{
    class MethodMapping
    {
        private readonly Dictionary<string, MethodInfo> _mappings = new Dictionary<string, MethodInfo>();
        private MethodInfo _default;

        /// <summary>
        /// Gets or sets if HTTP methods are specified.
        /// </summary>
        public bool IsMethodsSpecified { get; set; }

        public void Add(string method, MethodInfo info)
        {
            // Second call means that the user have
            // mapped a specific method.
            // copy the previous default one to 
            // get.
            if (_default != null)
                _mappings[Method.Get] = _default;
            else
                _default = info;

            _mappings.Add(method, info);
        }

        public MethodInfo Get(string method)
        {
            if (!IsMethodsSpecified)
                return _default;

            MethodInfo mi;
            return _mappings.TryGetValue(method, out mi) ? mi : null;
        }
    }
}