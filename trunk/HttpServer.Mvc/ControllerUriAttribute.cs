using System;

namespace HttpServer.Mvc
{
    /// <summary>
    /// To use another name than the class name
    /// </summary>
    [Obsolete("User ControllerUriAttribute instead.")]
    public class ControllerNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerNameAttribute"/> class.
        /// </summary>
        /// <param name="name">Controller name.</param>
        public ControllerNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets controller name.
        /// </summary>
        public string Name { get; private set; }


    }

    /// <summary>
    /// To use another name than the class name
    /// </summary>
    public class ControllerUriAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerUriAttribute"/> class.
        /// </summary>
        /// <param name="uri">Uri to controller.</param>
        /// <remarks>
        /// The uri can either be controller name, or a nested uri such as "/admin/user/". You
        /// can still have a "admin" controller, but it may not contain an action called "/user/".
        /// </remarks>
        public ControllerUriAttribute(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// Gets controller uri.
        /// </summary>
        public string Uri { get; private set; }


    }
}