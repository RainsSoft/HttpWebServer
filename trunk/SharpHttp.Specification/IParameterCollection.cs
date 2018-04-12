using System.Collections;
using System.Collections.Generic;

namespace SharpWeb
{
    /// <summary>
    /// Collection of parameters used in either the query string or in a HTML form.
    /// </summary>
    public interface IParameterCollection : IEnumerable<IParameter>
    {
        /// <summary>
        /// Gets number of parameters.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets last value of an parameter.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>String if found; otherwise <c>null</c>.</returns>
        string this[string name] { get; }

        /// <summary>
        /// Get a parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IParameter Get(string name);

        /// <summary>
        /// Add a query string parameter.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Value</param>
        void Add(string name, string value);
    }
}