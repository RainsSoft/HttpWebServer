using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.Helpers;
using HttpServer.Tools.Properties;

namespace HttpServer.MVC2
{
    public class DataConverter
    {
        private readonly IParameterCollection _queryString;
        private readonly IParameterCollection _form;
        private ArrayParameterCollection _formArray;
        private ArrayParameterCollection _queryStringArray;

        public DataConverter(IParameterCollection queryString, IParameterCollection form)
        {
            _queryString = queryString;
            _form = form;
            _formArray = new ArrayParameterCollection(form);
            _queryStringArray = new ArrayParameterCollection(_queryString);
        }

        public void TryPopulateModel(object model, string modelName)
        {

            if (model == null)
                throw new ArgumentNullException("model");

            if (_form.Exists(modelName))
                TryPopulateModel(model, _form[modelName]);


        }

        public void TryPopulateModel(object model, IParameterCollection parameterCollection)
        {
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
