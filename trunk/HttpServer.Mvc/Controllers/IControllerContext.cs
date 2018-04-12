﻿using System;

namespace HttpServer.Mvc.Controllers
{
    /// <summary>
    /// Context used during action invocation and rendering.
    /// </summary>
    public interface IControllerContext
    {
        /// <summary>
        /// Gets or sets action name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Will be filled in by controller director if empty (i.e. default action).
        /// </para>
        /// <para>
        /// Controller can change it if any other action should be rendered.
        /// </para>
        /// </remarks>
        string ActionName { get; set; }

        /// <summary>
        /// Gets name of controller
        /// </summary>
        /// <remarks>
        /// Might include slashes if the controller is a nested controller.
        /// </remarks>
        string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets controller uri.
        /// </summary>
        /// <remarks>
        /// A controller doesn't necessarily have to use the "/controllerName/" uri,
        /// but can exist in sub folders like "/area/area2/controllerName". This feature
        /// is controlled by the <see cref="ControllerUriAttribute"/>.
        /// </remarks>
        string ControllerUri { get; set; }


        /// <summary>
        /// Get a parameter.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Parameter value if found; otherwise <c>null</c>.</returns>
        /// <remarks>
        /// Wrapper around query string and form values.
        /// </remarks>
        string this[string name] { get; }

        /// <summary>
        /// Gets or sets layout to render.
        /// </summary>
        /// <value>
        /// <c>null</c> if default layout should be used.
        /// </value>
        string LayoutName { get; set; }

        /// <summary>
        /// Gets current request context
        /// </summary>
        RequestContext RequestContext { get; }

        /// <summary>
        /// Gets or sets document title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets requested URI.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Gets path split up on slash.
        /// </summary>
        string[] UriSegments { get; }

        /// <summary>
        /// Gets or sets path and name of view to render, excluding file extension.
        /// </summary>
        string ViewPath { get; set; }
    }
}