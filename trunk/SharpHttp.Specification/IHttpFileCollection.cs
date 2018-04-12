using System.Collections.Generic;

namespace SharpWeb
{
    /// <summary>
    /// Collection of received files.
    /// </summary>
    public interface IHttpFileCollection : IEnumerable<IHttpFile>
    {
        /// <summary>
        /// Get a file
        /// </summary>
        /// <param name="name">Name in form</param>
        /// <returns>File if found; otherwise <c>null</c>.</returns>
        IHttpFile this[string name] { get; }

        /// <summary>
        /// Gets number of files
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Checks if a file exists.
        /// </summary>
        /// <param name="name">Name of the file (form item name)</param>
        /// <returns></returns>
        bool Contains(string name);

        /// <summary>
        /// Add a new file.
        /// </summary>
        /// <param name="file">File to add.</param>
        void Add(IHttpFile file);

        /// <summary>
        /// Remove all files from disk.
        /// </summary>
        void Clear();
    }
}