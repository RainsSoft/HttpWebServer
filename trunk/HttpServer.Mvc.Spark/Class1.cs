using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HttpServer.Mvc.Controllers;
using HttpServer.Mvc.Views;
using Spark;

namespace HttpServer.Mvc.Spark
{
	public class SparkEngine : IViewEngine, ISparkServiceContainer
	{
		private SparkViewEngine _spark;

		public SparkEngine()
		{
			_spark = new SparkViewEngine();
			_spark.Initialize(this);
		}
		
		/// <summary>
		/// Create a view.
		/// </summary>
		/// <param name="context">Context to render</param>
		public void Render(ControllerContext context, IViewData viewData, TextWriter writer)
		{
			_spark.
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a list of file extensions that this engine use.
		/// </summary>
		public IEnumerable<string> FileExtensions
		{
			get {
				return new string[] {"spark"}; }
		}
	}
}
