namespace Mages.Core
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    /// <summary>
    /// A collection of useful extensions for the engine.
    /// </summary>
    public static class EngineExtensions
    {
        /// <summary>
        /// Adds or replaces a function with the given name to the function layer.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The name of the function to add or replace.</param>
        /// <param name="function">The function to be integrated.</param>
        public static void SetFunction(this Engine engine, String name, Function function)
        {
            engine.Globals[name] = function;
        }

        /// <summary>
        /// Adds or replaces a function represented as a general delegate by wrapping
        /// it as a function with the given name.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The name of the function to add or replace.</param>
        /// <param name="function">The function to be wrapped.</param>
        public static void SetFunction(this Engine engine, String name, Delegate function)
        {
            engine.Globals[name] = function.WrapFunction();
        }

        /// <summary>
        /// Adds or replaces a function represented as a reflected method info by
        /// wrapping it as a function with the given name.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The name of the function to add or replace.</param>
        /// <param name="method">The function to be wrapped.</param>
        /// <param name="target">The optional target object of the method.</param>
        public static void SetFunction(this Engine engine, String name, MethodInfo method, Object target = null)
        {
            engine.Globals[name] = method.WrapFunction(target);
        }

        /// <summary>
        /// Adds or replaces an object represented as the MAGES primitive. This is
        /// either directly the given value or a wrapper around it.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The name of the constant to add or replace.</param>
        /// <param name="value">The value to interact with.</param>
        public static void SetConstant(this Engine engine, String name, Object value)
        {
            engine.Globals[name] = value.WrapObject();
        }

        /// <summary>
        /// Exposes all static methods and the type's constructors in the object
        /// that can be freely placed.
        /// </summary>
        /// <typeparam name="T">The type to expose.</typeparam>
        /// <param name="engine">The engine.</param>
        public static IPlacement SetStatic<T>(this Engine engine)
        {
            return engine.SetStatic(typeof(T));
        }

        /// <summary>
        /// Exposes all static methods and the type's constructors in the object
        /// that can be freely placed.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="type">The type to expose.</param>
        public static IPlacement SetStatic(this Engine engine, Type type)
        {
            var name = engine.Globals.Keys.FindName(type);
            var obj = type.Expose();
            return new Placement(engine, name, obj);
        }

        /// <summary>
        /// Exposes all types in the assembly that satisfy the optional condition
        /// in an object that can be freely placed.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="lib">The library containing the types to expose.</param>
        /// <param name="shouldInclude">The optional inclusion checker.</param>
        public static IPlacement SetStatic(this Engine engine, Assembly lib, Predicate<Type> shouldInclude = null)
        {
            var types = lib.GetTypes();
            var libNameParts = lib.GetName().Name.Split(new[] { '.', ',', ' ', '-', '+' }, StringSplitOptions.RemoveEmptyEntries);
            var libName = String.Join(String.Empty, libNameParts);
            var obj = new Dictionary<String, Object>();

            foreach (var type in types)
            {
                if (shouldInclude == null || shouldInclude.Invoke(type))
                {
                    var name = obj.Keys.FindName(type);
                    var value = type.Expose();
                    obj[name] = value;
                }
            }

            return new Placement(engine, libName, obj);
        }

        /// <summary>
        /// Exposes all types in an object that can be freely placed. Here no
        /// default name is given.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="types">The types to include.</param>
        public static IPlacement SetStatic(this Engine engine, IEnumerable<Type> types)
        {
            var obj = new Dictionary<String, Object>();

            foreach (var type in types)
            {
                var name = obj.Keys.FindName(type);
                var value = type.Expose();
                obj[name] = value;
            }

            return new Placement(engine, null, obj);
        }

        /// <summary>
        /// Finds the missing symbols (if any) in the given source.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="source">The source code to inspect.</param>
        /// <returns>The variable expressions pointing to the missing symbols.</returns>
        public static IEnumerable<VariableExpression> FindMissingSymbols(this Engine engine, String source)
        {
            var parser = engine.Parser;
            var ast = parser.ParseStatements(source);
            var symbols = ast.ToBlock().FindMissingSymbols();

            for (var i = symbols.Count - 1; i >= 0; i--)
            {
                var symbol = symbols[i];
                var name = symbol.Name;

                if (engine.Scope.ContainsKey(name) || engine.Globals.ContainsKey(name))
                {
                    symbols.RemoveAt(i);
                }
            }

            return symbols;
        }

        sealed class Placement : IPlacement
        {
            private readonly Engine _engine;
            private readonly String _name;
            private readonly IDictionary<String, Object> _obj;

            public Placement(Engine engine, String name, IDictionary<String, Object> obj)
            {
                _engine = engine;
                _name = name;
                _obj = obj;
            }

            public void WithName(String name)
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("The given name has to be non-empty.", "name");
                }

                _engine.Globals[name] = _obj;
            }

            public void WithDefaultName()
            {
                WithName(_name);
            }

            public void Scattered()
            {
                foreach (var item in _obj)
                {
                    _engine.Globals[item.Key] = item.Value;
                }
            }
        }
    }
}
