using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace HttpServer.Mvc.Views.TinyEngine
{
    /// <summary>
    /// A general code compiler used to compile objects at runtime.
    /// </summary>
    /// <remarks>
    /// This class is NOT thread safe.
    /// </remarks>
    public class Compiler
    {
        private readonly List<Type> _types = new List<Type>();
        private readonly List<Assembly> _assemblies = new List<Assembly>();
        private readonly List<string> _namespaces = new List<string>();
        private Assembly _generatedAssembly;
        private Type[] _createdTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Compiler"/> class.
        /// </summary>
        public Compiler()
        {
            Add(GetType());
        }

        /// <summary>
        /// Add a type that is used in the generated object
        /// </summary>
        /// <param name="type">the type</param>
        public void Add(Type type)
        {
            if (_types.Contains(type))
                return;

            _types.Add(type);

            if (type.BaseType != null)
                Add(type.BaseType);

            foreach (Type t in type.GetInterfaces())
                Add(t);

            if (!type.IsGenericType || type.IsGenericTypeDefinition) return;
            foreach (Type argument in type.GetGenericArguments())
                Add(argument);
        }

        /// <summary>
        /// Fills name spaces and assemblies with info from the added types.
        /// </summary>
        private void CheckTypes()
        {
            _assemblies.Clear();
            _namespaces.Clear();

            foreach (Type type in _types)
            {
                if (!_assemblies.Contains(type.Assembly))
                    _assemblies.Add(type.Assembly);
                if (!_namespaces.Contains(type.Namespace))
                    _namespaces.Add(type.Namespace);
            }
        }

        /// <summary>
        /// Compile the code.
        /// </summary>
        /// <param name="code">C# code.</param>
        /// <returns>An assembly containing the compiled objects</returns>
        /// <exception cref="InvalidOperationException">If compilation fails.</exception>
        /// <exception cref="CompilerException"><c>CompilerException</c>.</exception>
        public Assembly Compile(string code)
        {
            Add(typeof (MethodInfo));
            CheckTypes();

            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            foreach (Assembly assembly in _assemblies)
                parameters.ReferencedAssemblies.Add(assembly.Location);

            string namespaceStr = string.Empty;
            foreach (string ns in _namespaces)
                namespaceStr += "using " + ns + ";\r\n";
            code = namespaceStr + code;

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.Count > 0)
            {
                string errs = "";

                foreach (CompilerError CompErr in results.Errors)
                    errs += "Line #" + CompErr.Line + ", " + CompErr.ErrorNumber + " '" + CompErr.ErrorText + "'" +
                            Environment.NewLine;
                var err = new CompilerException(errs, results.Errors);
                err.Data.Add("code", code);
                throw err;
            }

            _generatedAssembly = results.CompiledAssembly;
            _createdTypes = _generatedAssembly.GetTypes();
            return _generatedAssembly;
        }

        /// <summary>
        /// Create a new object of the specified type
        /// </summary>
        /// <param name="contructorArguments">Constructor parameters</param>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <returns>object if we could create it; otherwise null</returns>
        public T CreateInstance<T>(params object[] contructorArguments)
        {
            if (_createdTypes.Length == 1)
                return
                    (T)
                    _generatedAssembly.CreateInstance(_createdTypes[0].FullName, false, BindingFlags.CreateInstance,
                                                      null, contructorArguments, null, null);

            Type type = typeof (T);

            // check of exact matches.
            foreach (Type createdType in _createdTypes)
            {
                if (type == createdType)
                    return
                        (T)
                        _generatedAssembly.CreateInstance(_createdTypes[0].FullName, false, BindingFlags.CreateInstance,
                                                          null, contructorArguments, null, null);
            }

            // check assignable from
            foreach (Type createdType in _createdTypes)
            {
                if (type.IsAssignableFrom(createdType))
                    return
                        (T)
                        _generatedAssembly.CreateInstance(_createdTypes[0].FullName, false, BindingFlags.CreateInstance,
                                                          null, contructorArguments, null, null);
            }

            return default(T);
        }

        /// <summary>
        /// Used to get correct names for generics.
        /// </summary>
        /// <param name="type">Type to generate a strig name for.</param>
        /// <returns>Type as a code string</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string typeName = typeof(List<string>).Name; // will become: List`1
        /// typeName = Compiler.GetTypeName(typeof(List<string>)); // will become: System.Collections.Generic.List<string>
        /// ]]>
        /// </code>
        /// </example>
        public static string GetTypeName(Type type)
        {
            return GetTypeName(type, true);
        }

        /// <summary>
        /// Used to get correct names for generics.
        /// </summary>
        /// <param name="type">Type to generate a strig name for.</param>
        /// <param name="useFullName">true if FullName should be used (including namespace in typename)</param>
        /// <returns>Type as a code string</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string typeName = typeof(List<string>).Name; // will become: List`1
        /// typeName = Compiler.GetTypeName(typeof(List<string>)); // will become: System.Collections.Generic.List<string>
        /// ]]>
        /// </code>
        /// </example>
        public static string GetTypeName(Type type, bool useFullName)
        {
            string typeName = useFullName ? type.FullName : type.Name;
            if (type.IsNested)
                typeName = typeName.Replace('+', '.');

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var sb = new StringBuilder();
                sb.AppendFormat(typeName.Substring(0, typeName.IndexOf('`')));
                sb.Append("<");
                bool first = true;
                foreach (Type genericArgumentType in type.GetGenericArguments())
                {
                    if (!first)
                        sb.Append(", ");
                    first = false;
                    sb.Append(GetTypeName(genericArgumentType, useFullName));
                }
                sb.Append(">");
                return sb.ToString();
            }

            return typeName;
        }

#if TEST
        [Fact]
        private static void TestGetTypeName()
        {
            Assert.Equal("System.String", GetTypeName(typeof(string)));
            Assert.Equal("System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, Fadd.MonthSpan>>", GetTypeName(typeof(List<Dictionary<string, MonthSpan>>)));
            Assert.Equal("String", GetTypeName(typeof(string), false));
            Assert.Equal("List<Dictionary<String, MonthSpan>>", GetTypeName(typeof(List<Dictionary<string, MonthSpan>>), false));
        }
#endif
    }

    /// <summary>
    /// Thrown if code compilation fails.
    /// </summary>
    /// <remarks>
    /// <see cref="Exception.Data"/> contains an item called <c>code</c> which contains the compiled code.
    /// </remarks>
    public class CompilerException : Exception
    {
        private readonly CompilerErrorCollection _errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerException"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorCollection">A list with all compiler generated errors.</param>
        public CompilerException(string errorMessage, CompilerErrorCollection errorCollection)
            : base(errorMessage)
        {
            _errors = errorCollection;
        }

        /// <summary>
        /// A list with all compiler generated errors.
        /// </summary>
        public CompilerErrorCollection Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>
        /// A string representation of the current exception.
        /// </returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override string ToString()
        {
            return Message;
        }
    }
}