namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    static class Helpers
    {
        public static readonly TypeConverterMap Converters = new TypeConverterMap();
        private static readonly Object[] Empty = new Object[0];

        public static IEnumerable YieldAround(this IEnumerable enu, Function sel)
        {
            foreach (var item in enu)
                yield return sel(new[] { item });
        }

        public static IEnumerable YieldAround<T>(this IEnumerable enu, Func<object, T> sel)
        {
            foreach (var item in enu)
                yield return sel(item);
        }

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

        public static Object GetKeys(this IDictionary<String, Object> obj)
        {
            var result = new Dictionary<String, Object>();

            foreach (var item in obj)
            {
                result[result.Count.ToString()] = item.Key;
            }

            return result;
        }

        public static Boolean Satisfies(this IDictionary<String, Object> constraints, Object value)
        {
            var obj = value as IDictionary<String, Object>;

            if (obj != null && obj.Count >= constraints.Count)
            {
                foreach (var constraint in constraints)
                {
                    var val = default(Object);

                    if (obj.TryGetValue(constraint.Key, out val))
                    {
                        var simple = constraint.Value as String;
                        var extended = constraint.Value as IDictionary<String, Object>;

                        if ((simple == null || val.ToType() == simple) &&
                            (extended == null || extended.Satisfies(val)))
                        {
                            continue;
                        }
                    }
                    
                    return false;
                }

                return true;
            }

            return false;
        }

        public static Boolean AllTrue(this IDictionary<String, Object> obj)
        {
            foreach (var item in obj)
            {
                if (!item.Value.ToBoolean())
                {
                    return false;
                }
            }

            return obj.Count > 0;
        }

        public static Boolean AnyTrue(this IDictionary<String, Object> obj)
        {
            foreach (var item in obj)
            {
                if (item.Value.ToBoolean())
                {
                    return true;
                }
            }

            return false;
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

        public static Object Zip(this IDictionary<String, Object> a, IDictionary<String, Object> b)
        {
            var result = new Dictionary<String, Object>();
            var index = 0;
            var aiter = a.Values.GetEnumerator();
            var biter = b.Values.GetEnumerator();

            while (aiter.MoveNext() && biter.MoveNext())
            {
                var value = new[] { aiter.Current, biter.Current };
                result[(index++).ToString()] = ToArrayObject(value);
            }

            return result;
        }

        public static Object Merge(this IDictionary<String, Object> a, IDictionary<String, Object> b)
        {
            var result = new Dictionary<String, Object>();
            var index = 0;

            foreach (var value in a.Values)
            {
                result[(index++).ToString()] = value;
            }

            foreach (var value in b.Values)
            {
                result[(index++).ToString()] = value;
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

        public static IDictionary<String, Object> WrapArray(this Array array)
        {
            var l = array.Length;
            var o = new Object[l];

            for (var i = 0; i < l; i++)
            {
                o[i] = array.GetValue(i).WrapObject();
            }

            return o.ToArrayObject();
        }

        public static Function WrapFunction(this MethodInfo method, Object target)
        {
            var parameters = method.GetParameters();
            var f = default(Function);

            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(Object[]) || method.ReturnType != typeof(Object))
            {
                f = new Function(args =>
                {
                    var result = Curry.Min(parameters.Length, f, args);

                    if (result == null && method.TryMatch(parameters, ref args))
                    {
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
                var target = type.FindPrimitive();
                var converter = Converters.FindConverter(type, target);
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
