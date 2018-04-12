using System;
using System.Collections.Generic;
using System.IO;
using HttpServer.Logging;
using HttpServer.Mvc;
using HttpServer.Resources;
using Spark.FileSystem;

namespace HttpServer.ViewEngine.Spark
{
    internal class MyViewFolder : IViewFolder
    {
        private ILogger _logger = LogFactory.CreateLogger(typeof (MyViewFolder));

        private string GetOurPath(string path)
        {
            path = path.Replace('\\', '/').ToLower();
            if (!path.StartsWith("/"))
                path = "/" + path;
            return path;
        }

        #region IViewFolder Members

        public IViewFile GetViewSource(string path)
        {
            path = GetOurPath(path);
            _logger.Trace("GetViewSource '" + path + "'.");
            Resource resource = MvcServer.CurrentMvc.ViewProvider.Get(path);
            if (resource == null)
                _logger.Error("Failed to get source for view: " + path);
            return resource == null ? null : new ViewFile(resource);
        }

        public IList<string> ListViews(string path)
        {
            if (string.IsNullOrEmpty(path))
                return new List<string>();
            path = GetOurPath(path) + "/";
            _logger.Trace("ListViews: " + path);
            IList<string> items = MvcServer.CurrentMvc.ViewProvider.Find(GetOurPath(path));
            return items;
        }

        public bool HasView(string path)
        {
            path = GetOurPath(path);
            bool res = MvcServer.CurrentMvc.ViewProvider.Exists(path);
            _logger.Trace("HasView: " + path + " = " + res);
            return res;
        }

        #endregion

        #region Nested type: ViewFile

        private class ViewFile : IViewFile
        {
            private readonly Resource _resource;

            public ViewFile(Resource resource)
            {
                _resource = resource;
            }

            #region IViewFile Members

            public Stream OpenViewStream()
            {
                return _resource.Stream;
            }

            public long LastModified
            {
                get { return _resource.ModifiedAt.Ticks; }
            }

            #endregion
        }

        #endregion
    }
}