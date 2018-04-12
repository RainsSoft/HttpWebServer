using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.Mvc.Spark.mvc;
using Spark;
using Spark.Web.Mvc;

namespace HttpServer.Mvc.Spark
{
    class Shit
    {
        public void Configure()
        {
            SparkSettings settings = new SparkSettings();
            
            SparkViewFactory viewFactory = new SparkViewFactory(settings) {ViewFolder = new MyViewFolder()};

            var container = new SparkServiceContainer(settings);
            ConfigureContainer(container);
            return container;            
        }
    }
}
