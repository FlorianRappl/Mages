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
            var result = Math.Sign(value);
            var n = (Int32)Math.Floor(result * value);

            while (n > 0)
            {
                result *= n--;
            }

            return result;
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
            var type = value.GetType();
            var primitive = type.FindPrimitive();
            var converter = Converters.FindConverter(type, primitive);
            return converter.Invoke(value);
        }
    }
}
