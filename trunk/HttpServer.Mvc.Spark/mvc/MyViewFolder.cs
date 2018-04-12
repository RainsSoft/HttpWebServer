using System.Collections.Generic;
using System.IO;
using HttpServer.Resources;
using Spark.FileSystem;

namespace HttpServer.Mvc.Spark.mvc
{
    internal class MyViewFolder : IViewFolder
    {
        #region IViewFolder Members

        public IViewFile GetViewSource(string path)
        {
            Resource resource = MvcServer.CurrentMvc.ViewProvider.Get(path);
            return new ViewFile(resource);
        }

        public IList<string> ListViews(string path)
        {
            return new List<string>();
        }

        public bool HasView(string path)
        {
            return MvcServer.CurrentMvc.ViewProvider.Exists(path);
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
                get { return _resource.ModifiedAt.ToFileTime(); }
            }

            #endregion
        }

        #endregion
    }
}