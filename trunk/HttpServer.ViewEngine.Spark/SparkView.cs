using System;
using HttpServer.Mvc;
using Spark;

namespace HttpServer.ViewEngine.Spark
{
    /// <summary>
    /// Our view implementation.
    /// </summary>
    public abstract class SparkView : SparkViewBase
    {
        private IViewData _viewData;

        /// <summary>
        /// Gets or sets view data.
        /// </summary>
        public IViewData ViewData
        {
            get { return _viewData; }
            set { _viewData = value; }
        }

        /// <summary>
        /// Gets or sets current Uri.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets controller name.
        /// </summary>
        public string ControllerName { get; internal set; }

        /// <summary>
        /// Gets URI to controller
        /// </summary>
        /// 
        public string ControllerUri { get; internal set; }

        /// <summary>
        /// Gets or sets action name.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets document title.
        /// </summary>
        public string Title { get; set; }

        public virtual object Eval(string expression)
        {
            return _viewData[expression];
        }

        public string Eval(string expression, string format)
        {
            return _viewData[expression].ToString();
        }

        public override bool TryGetViewData(string name, out object value)
        {
            return ViewData.TryGetValue(name, out value);
        }
    }
}