using System;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Add a view dependency
    /// </summary>
    /// <remarks>
    /// Tag a controller or action with this attribute to add
    /// a view dependency to the specified type.
    /// </remarks>
    public class ViewDependencyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewDependencyAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ViewDependencyAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets type that the view is dependent of.
        /// </summary>
        public Type Type { get; private set; }
    }
}