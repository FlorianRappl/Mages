namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The collection of all type function creators.
    /// </summary>
    public static class TypeFunctions
    {
        private static Dictionary<Type, Func<Object, Function>> _getters = new Dictionary<Type, Func<Object, Function>>
        {
            { typeof(Double[,]), obj => ((Double[,])obj).Getter },
            { typeof(String), obj => ((String)obj).Getter },
            { typeof(IDictionary<String, Object>), obj => ((IDictionary<String, Object>)obj).Getter },
        };


        private static Dictionary<Type, Func<Object, Procedure>> _setters = new Dictionary<Type, Func<Object, Procedure>>
        {
            { typeof(Double[,]), obj => ((Double[,])obj).Setter },
        };

        /// <summary>
        /// Registers the provided getter function.
        /// </summary>
        /// <typeparam name="T">The type of the object to extend.</typeparam>
        /// <param name="getter">The getter function to register.</param>
        public static void RegisterGetter<T>(Func<T, Function> getter)
        {
            _getters[typeof(T)] = val => getter((T)val);
        }

        /// <summary>
        /// Registers the provided setter procedure.
        /// </summary>
        /// <typeparam name="T">The type of the object to extend.</typeparam>
        /// <param name="setter">The setter function to register.</param>
        public static void RegisterSetter<T>(Func<T, Procedure> setter)
        {
            _setters[typeof(T)] = val => setter((T)val);
        }

        /// <summary>
        /// Tries to find the named getter.
        /// </summary>
        /// <param name="instance">The object context.</param>
        /// <param name="function">The potentially found getter function.</param>
        /// <returns>True if the getter could be found, otherwise false.</returns>
        public static Boolean TryFindGetter(Object instance, out Function function)
        {
            var type = instance.GetType();

            foreach (var getter in _getters)
            {
                if (getter.Key.IsAssignableFrom(type))
                {
                    function = getter.Value.Invoke(instance);
                    return true;
                }
            }

            function = null;
            return false;
        }

        /// <summary>
        /// Tries to find the named setter.
        /// </summary>
        /// <param name="instance">The object context.</param>
        /// <param name="procedure">The potentially found setter procedure.</param>
        /// <returns>True if the setter could be found, otherwise false.</returns>
        public static Boolean TryFindSetter(Object instance, out Procedure procedure)
        {
            var type = instance.GetType();

            foreach (var setter in _setters)
            {
                if (setter.Key.IsAssignableFrom(type))
                {
                    procedure = setter.Value.Invoke(instance);
                    return true;
                }
            }

            procedure = null;
            return false;
        }
    }
}
