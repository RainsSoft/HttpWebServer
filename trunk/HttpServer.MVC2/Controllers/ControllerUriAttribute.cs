using System;

namespace HttpServer.MVC2.Controllers
{
    /// <summary>
    /// Used to determine the uri of the controller.
    /// </summary>
    /// <remarks>
    /// Asp.NET MVC uses areas while we just point out the root uri for the controller.
    /// Using the attribute overrides the actual controller name, hence you must include
    /// the controller name in the uri.
    /// </remarks>
    /// <example>
    /// <code>
    /// [ControllerUri("/admin/user/")];
    /// public UserController : Controller
    /// {
    /// }
    /// </code>
    /// </example>
    public class ControllerUriAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerUriAttribute"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public ControllerUriAttribute(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// Gets or sets the controller URI.
        /// </summary>
        /// <value>The URI.</value>
        public string Uri { get; set; }
    }
}