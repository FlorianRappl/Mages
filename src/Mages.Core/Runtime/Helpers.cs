namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    static class Helpers
    {
        public static readonly TypeConverterMap Converters = new TypeConverterMap();

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

        public static Object Expose(Type type)
        {
            return new WrapperObject(type);
        }

        public static Function WrapFunction(Delegate func)
        {
            return WrapFunction(func.Method, func.Target);
        }

        public static Function WrapFunction(MethodInfo method, Object target)
        {
            var parameterConverters = method.GetParameters().Select(m => Converters.FindConverter(m.ParameterType)).ToArray();
            var returnType = method.ReturnType;
            var returnConverter = Converters.FindConverter(returnType, returnType.FindPrimitive());
            return new Function(args =>
            {
                if (args.Length >= parameterConverters.Length)
                {
                    for (var i = 0; i < parameterConverters.Length; i++)
                    {
                        args[i] = parameterConverters[i].Invoke(args[i]);
                    }

                    var result = method.Invoke(target, args);
                    return returnConverter.Invoke(result);
                }

                return null;
            });
        }

        public static Object WrapObject(Object value)
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
    }
}
