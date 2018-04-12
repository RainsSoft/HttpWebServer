using System;
using System.Collections.Generic;
using System.Text;

namespace SharpWeb
{
    public interface IServiceResolver
    {
        T Resolve<T>();
        T Resolve<T>(params ConstructorParameter[] constructorParameters);
    }

    public class ConstructorParameter
    {
        public ConstructorParameter(object value)
        {
            
        }
        public ConstructorParameter(string name, object value)
        {

        }
    }
}
