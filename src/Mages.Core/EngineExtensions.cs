namespace Mages.Core
{
    using Mages.Core.Runtime;
    using System;
    using System.Reflection;

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
        /// Adds or replaces a function represented as a general delegate by wrapping it as
        /// a function with the given name.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The name of the function to add or replace.</param>
        /// <param name="function">The function to be wrapped.</param>
        public static void SetFunction(this Engine engine, String name, Delegate function)
        {
            engine.Globals[name] = Helpers.WrapFunction(function);
        }

        /// <summary>
        /// Adds or replaces a function represented as a reflected method info by wrapping it
        /// as a function with the given name.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The name of the function to add or replace.</param>
        /// <param name="method">The function to be wrapped.</param>
        /// <param name="target">The optional target object of the method.</param>
        public static void SetFunction(this Engine engine, String name, MethodInfo method, Object target = null)
        {
            engine.Globals[name] = Helpers.WrapFunction(method, target);
        }

        /// <summary>
        /// Adds or replaces an object represented as the MAGES primitive. This is either directly
        /// the given value or a wrapper around it.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The name of the constant to add or replace.</param>
        /// <param name="value">The value to interact with.</param>
        public static void SetConstant(this Engine engine, String name, Object value)
        {
            engine.Globals[name] = Helpers.WrapObject(value);
        }
    }
}
