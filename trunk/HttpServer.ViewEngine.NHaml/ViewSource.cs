using System.IO;
using HttpServer.Resources;
using NHaml.TemplateResolution;

namespace HttpServer.ViewEngine.NHaml
{
    /// <summary>
    /// Information about a view file
    /// </summary>
    /// <remarks>
    /// NHaml uses ViewSource to get information about a view file.  Let's just
    /// wrap our own Resource object.
    /// </remarks>
    public class ViewSource : IViewSource
    {
        private readonly string _path;
        private readonly Resource _resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewSource"/> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="path">The path.</param>
        public ViewSource(Resource resource, string path)
        {
            _resource = resource;
            _path = path;
        }

        #region IViewSource Members

        /// <summary>
        /// Gets the stream reader.
        /// </summary>
        /// <returns></returns>
        public StreamReader GetStreamReader()
        {
            return new StreamReader(_resource.Stream);
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get { return false; }
        }

        #endregion
    }
}