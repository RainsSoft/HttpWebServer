using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.Helpers;
using HttpServer.Tools.Properties;

namespace HttpServer.MVC2.Models
{
    public class ModelLoader
    {
        public void Load(object model, IParameterCollection parameterCollection)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            if (parameterCollection == null)
                throw new ArgumentNullException("parameterCollection");
            var errors = new Dictionary<string, Exception>();
            ICachedType type = PropertyProvider.Get(model.GetType());
            foreach (IParameter parameter in parameterCollection)
            {
                try
                {
                    object value = parameter.Value;
                    type.SetConvertedValue(model, parameter.Name, value);
                }
                catch (Exception err)
                {
                    errors[parameter.Name] = err;
                }
            }

            if (errors.Count != 0)
                throw new PropertyException(errors);
        }
    }
}
