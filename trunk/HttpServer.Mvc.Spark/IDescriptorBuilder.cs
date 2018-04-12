using System.Collections.Generic;
using System.Linq;
using HttpServer.Mvc.Controllers;

namespace Spark.Web.Mvc
{
    public interface IDescriptorBuilder
    {
        /// <summary>
        /// Implemented by custom descriptor builder to quickly extract additional parameters needed
        /// to locate templates, like the theme or language in effect for the request
        /// </summary>
        /// <param name="controllerContext">Context information for the current request</param>
        /// <returns>An in-order array of values which are meaningful to BuildDescriptor on the same implementation class</returns>
        IDictionary<string, object> GetExtraParameters(ControllerContext controllerContext);

        /// <summary>
        /// Given a set of MVC-specific parameters, a descriptor for the target view is created. This can
        /// be a bit more expensive because the existence of files is tested at various candidate locations.
        /// </summary>
        /// <param name="buildDescriptorParams">Contains all of the standard and extra parameters which contribute to a descriptor</param>
        /// <param name="searchedLocations">Candidate locations are added to this collection so an information-rich error may be returned</param>
        /// <returns>The descriptor with all of the detected view locations in order</returns>
        SparkViewDescriptor BuildDescriptor(BuildDescriptorParams buildDescriptorParams, ICollection<string> searchedLocations);
    }

    public class BuildDescriptorParams
    {
    	private static readonly IDictionary<string, object> _extraEmpty = new Dictionary<string, object>();
        private readonly int _hashCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildDescriptorParams"/> class.
		/// </summary>
		/// <param name="targetNamespace">The target namespace.</param>
		/// <param name="controllerName">Name of the controller.</param>
		/// <param name="viewName">Name of the view.</param>
		/// <param name="masterName">Name of the master.</param>
		/// <param name="findDefaultMaster">if set to <c>true</c> [find default master].</param>
		/// <param name="extra">The extra.</param>
        public BuildDescriptorParams(string targetNamespace, string controllerName, string viewName, string masterName, bool findDefaultMaster, IDictionary<string, object> extra)
        {
            TargetNamespace = targetNamespace;
            ControllerName = controllerName;
            ViewName = viewName;
            LayoutName = masterName;
            FindDefaultMaster = findDefaultMaster;
            Extra = extra ?? _extraEmpty;

            // this object is meant to be immutable and used in a dictionary.
            // the hash code will always be used so it isn't calculated just-in-time.
            _hashCode = CalculateHashCode();
        }

    	public string TargetNamespace { get; private set; }

    	public string ControllerName { get; private set; }

    	public string ViewName { get; private set; }

    	public string LayoutName { get; private set; }

    	public bool FindDefaultMaster { get; private set; }

    	public IDictionary<string, object> Extra { get; private set; }

    	private static int Hash(object str)
        {
            return str == null ? 0 : str.GetHashCode();
        }

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }

		/// <summary>
		/// Calculates hash code using view name, controller name, target name space
		/// master name and default master hash code.
		/// </summary>
		/// <returns></returns>
        private int CalculateHashCode()
        {
            return Hash(ViewName) ^
                   Hash(ControllerName) ^
                   Hash(TargetNamespace) ^
                   Hash(LayoutName) ^
                   FindDefaultMaster.GetHashCode() ^
                   Extra.Aggregate(0, (hash, kv) => hash ^ Hash(kv.Key) ^ Hash(kv.Value));
        }

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <paramref name="obj"/> parameter is <c>null</c>.
		/// </exception>
        public override bool Equals(object obj)
        {
            var that = obj as BuildDescriptorParams;
            if (that == null || that.GetType() != GetType())
                return false;

            if (!string.Equals(ViewName, that.ViewName) ||
                !string.Equals(ControllerName, that.ControllerName) ||
                !string.Equals(TargetNamespace, that.TargetNamespace) ||
                !string.Equals(LayoutName, that.LayoutName) ||
                FindDefaultMaster != that.FindDefaultMaster ||
                Extra.Count != that.Extra.Count)
            {
                return false;
            }
            foreach (var kv in Extra)
            {
                object value;
                if (!that.Extra.TryGetValue(kv.Key, out value) ||
                    !Equals(kv.Value, value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
