using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HttpServer.Mvc.Controllers;
using HttpServer.Mvc.Views;
using Spark.FileSystem;
using Spark.Web.Mvc.Wrappers;

namespace HttpServer.Mvc.Spark.mvc
{
    class ViewFactory : IViewEngine, IViewFolderContainer
    {
        /// <summary>
        /// Create a view.
        /// </summary>
        /// <param name="context">Context to render</param>
        public void Render(ControllerContext context, IViewData viewData, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of file extensions that this engine use.
        /// </summary>
        public IEnumerable<string> FileExtensions
        {
            get { throw new NotImplementedException(); }
        }

        public IViewFolder ViewFolder
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
