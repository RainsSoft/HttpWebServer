using System.Collections.Generic;
using System.Reflection;

namespace HttpServer.ViewEngine.Spark
{
    public class SparkEngineSettings
    {
        /// <summary>
        /// Gets or sets assemblies
        /// </summary>
        public List<Assembly> Assemblies { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        public List<string> Namespaces { get; set; }
    }
}
