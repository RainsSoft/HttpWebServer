﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.Mvc.Controllers;
using Spark;
using Spark.Web.Mvc;

namespace HttpServer.Mvc.Spark.mvc
{
    class DescriptorBuilder : IDescriptorBuilder, ISparkServiceInitialize
    {
        /// <summary>
        /// Implemented by custom descriptor builder to quickly extract additional parameters needed
        /// to locate templates, like the theme or language in effect for the request
        /// </summary>
        /// <param name="controllerContext">Context information for the current request</param>
        /// <returns>An in-order array of values which are meaningful to BuildDescriptor on the same implementation class</returns>
        public IDictionary<string, object> GetExtraParameters(ControllerContext controllerContext)
        {
            return null;
        }

        /// <summary>
        /// Given a set of MVC-specific parameters, a descriptor for the target view is created. This can
        /// be a bit more expensive because the existence of files is tested at various candidate locations.
        /// </summary>
        /// <param name="buildDescriptorParams">Contains all of the standard and extra parameters which contribute to a descriptor</param>
        /// <param name="searchedLocations">Candidate locations are added to this collection so an information-rich error may be returned</param>
        /// <returns>The descriptor with all of the detected view locations in order</returns>
        public SparkViewDescriptor BuildDescriptor(BuildDescriptorParams buildDescriptorParams, ICollection<string> searchedLocations)
        {
            return null;
        }

        public void Initialize(ISparkServiceContainer container)
        {
            
        }
    }
}
