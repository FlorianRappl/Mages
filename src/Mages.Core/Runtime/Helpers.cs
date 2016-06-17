namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    static class Helpers
    {
        public static readonly TypeConverterMap Converters = new TypeConverterMap();
        private static readonly Object[] Empty = new Object[0];

        public static Object Getter(this String str, Object[] arguments)
        {
            var index = 0;

            if (arguments.Length > 0 && arguments[0].TryGetIndex(out index) && index >= 0 && index < str.Length)
            {
                return new String(str[index], 1);
            }

            return null;
        }

        public static Object Getter(this IDictionary<String, Object> obj, Object[] arguments)
        {
            if (arguments.Length > 0)
            {
                var name = Stringify.This(arguments[0]);
                return obj.GetProperty(name);
            }

            return null;
        }

        public static void Setter(this IDictionary<String, Object> obj, Object[] arguments, Object value)
        {
            if (arguments.Length > 0)
            {
                var name = Stringify.This(arguments[0]);
                obj.SetProperty(name, value);
            }
        }

        public static void SetProperty(this IDictionary<String, Object> obj, String name, Object value)
        {
            obj[name] = value;
        }

        public static Object GetProperty(this IDictionary<String, Object> obj, String name)
        {
            var value = default(Object);
            obj.TryGetValue(name, out value);
            return value;
        }

        public static Object Map(this IDictionary<String, Object> obj, Function f)
        {
            var result = new Dictionary<String, Object>();
            var args = new Object[2];

            foreach (var item in obj)
            {
                args[0] = item.Value;
                args[1] = item.Key;
                result[item.Key] = f(args);
            }

            return result;
        }

        public static Object Where(this IDictionary<String, Object> obj, Function f)
        {
            var result = new Dictionary<String, Object>();
            var args = new Object[2];

            foreach (var item in obj)
            {
                args[0] = item.Value;
                args[1] = item.Key;

                if (f(args).ToBoolean())
                {
                    result[item.Key] = item.Value;
                }
            }

            return result;
        }

        public static Object Reduce(this IDictionary<String, Object> obj, Function f, Object start)
        {
            var result = start;
            var args = new Object[3];

            foreach (var item in obj)
            {
                args[0] = result;
                args[1] = item.Value;
                args[2] = item.Key;
                result = f(args);
            }

            return result;
        }

        public static IDictionary<String, Object> ToArrayObject(this Object[] arguments)
        {
            var obj = new Dictionary<String, Object>();

            for (var i = 0; i < arguments.Length; i++)
            {
                obj[i.ToString()] = arguments[i];
            }

            return obj;
        }

        public static Double Sign(this Double value)
        {
            return (Double)Math.Sign(value);
        }

        public static Double Factorial(this Double value)
        {
            var result = (Double)Math.Sign(value);
            var n = (Int32)Math.Floor(result * value);

            while (n > 0)
            {
                result *= n--;
            }

            return result;
        }

        public static WrapperObject Expose(this Type type)
        {
            return WrapperObject.CreateFor(type);
        }

        public static String FindName(this IEnumerable<String> names, MemberInfo member)
        {
            var selector = Container.GetService<INameSelector>(CamelNameSelector.Instance);
            return selector.Select(names, member);
        }

        public static Function WrapFunction(this Delegate func)
        {
            return WrapFunction(func.Method, func.Target);
        }

        public static Function WrapFunction(this MethodInfo method, Object target)
        {
            var parameters = method.GetParameters();
            var f = default(Function);

            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(Object[]) || method.ReturnType != typeof(Object))
            {
                var parameterConverters = parameters.Select(m => Converters.FindConverter(m.ParameterType)).ToArray();

                f = new Function(args =>
                {
                    var result = Curry.Min(parameterConverters.Length, f, args);

                    if (result == null)
                    {
                        for (var i = 0; i < parameterConverters.Length; i++)
                        {
                            args[i] = parameterConverters[i].Invoke(args[i]);
                        }

                        result = method.Call(target, args);
                    }

                    return result;
                });
            }
            else
            {
                f = (Function)Delegate.CreateDelegate(typeof(Function), target, method);
            }

            return f;
        }

        public static Object WrapObject(this Object value)
        {
            if (value != null)
            {
                var type = value.GetType();
                var primitive = type.FindPrimitive();
                var converter = Converters.FindConverter(type, primitive);
                return converter.Invoke(value);
            }

            return null;
        }

        public static Object Unwrap(this Object value)
        {
            var wrapped = value as WrapperObject;

            if (wrapped != null)
            {
                return wrapped.Content ?? wrapped.Type;
            }

            return value;
        }

        public static Object SafeExecute(Function function)
        {
            var result = new Dictionary<String, Object>
            {
                { "value", null },
                { "error", null }
            };

            try
            {
                result["value"] = function.Invoke(Empty);
            }
            catch (Exception ex)
            {
                result["error"] = ex.Message;
            }

            return result;
        }
    }
}
