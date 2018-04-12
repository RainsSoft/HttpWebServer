using System;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Defines which layout (master page in ASP) that the action or controller should use.
    /// </summary>
    public class LayoutAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutAttribute"/> class.
        /// </summary>
        /// <param name="name">Name of layout.</param>
        public LayoutAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets name of layout
        /// </summary>
        public string Name { get; private set; }
    }
}