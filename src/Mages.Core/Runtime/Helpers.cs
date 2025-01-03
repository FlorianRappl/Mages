﻿namespace Mages.Core.Runtime;

using Mages.Core.Runtime.Converters;
using Mages.Core.Runtime.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

static class Helpers
{
    public static readonly TypeConverterMap Converters = new();
    private static readonly Object[] Empty = [];

    public static Object Getter(this String str, Object[] arguments)
    {
        if (arguments.Length > 0 && arguments[0].TryGetIndex(out var index) && index >= 0 && index < str.Length)
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
        obj.TryGetValue(name, out var value);
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

    public static String Clamp(this String value, Int32 min, Int32 max)
    {
        if (value.Length < min)
        {
            return value.PadRight(min, ' ');
        }
        else if (value.Length > max)
        {
            return value.Substring(0, max);
        }
        else
        {
            return value;
        }
    }

    public static String Clip(this String value, Int32 from, Int32 to)
    {
        if (to < from)
        {
            return String.Empty;
        }
        else if (value.Length < from)
        {
            return String.Empty.PadLeft(1 + to - from);
        }
        else if (value.Length > to)
        {
            return value.Substring(from, 1 + to - from);
        }
        else if (value.Length <= to)
        {
            return value.Substring(from, value.Length - from);
        }
        else
        {
            return value;
        }
    }

    public static Double Clamp(this Double value, Double min, Double max) => Math.Max(Math.Min(value, max), min);

    public static Double Lerp(this Double value, Double min, Double max)
    {
        var rho = value.Clamp(0.0, 1.0);
        var delta = max - min;
        return rho * delta + min;
    }

    public static Boolean Satisfies(this IDictionary<String, Object> constraints, Object value)
    {
        var obj = value as IDictionary<String, Object>;

        if (obj is not null && obj.Count >= constraints.Count)
        {
            foreach (var constraint in constraints)
            {
                if (obj.TryGetValue(constraint.Key, out var val))
                {
                    var simple = constraint.Value as String;
                    var extended = constraint.Value as IDictionary<String, Object>;

                    if ((simple is null || val.ToType()["name"].ToString() == simple) &&
                        (extended is null || extended.Satisfies(val)))
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

    public static void ContinueWith(this Future promise, Future future)
    {
        promise.SetCallback((result, error) =>
        {
            if (result is Future nestedPromise)
            {
                nestedPromise.ContinueWith(future);
            }
            else if (error is not null)
            {
                future.SetError(error);
            }
            else
            {
                future.SetResult(result);
            }
        });
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

    public static IDictionary<String, Object> ToJsxObject(this Object[] arguments)
    {
        var obj = new Dictionary<String, Object>
        {
            ["type"] = arguments.Length > 0 ? arguments[0] : null,
            ["props"] = arguments.Length > 1 ? arguments[1] : null,
        };

        if (arguments.Length > 2 && arguments[2] is IDictionary<String, Object> array && obj["props"] is IDictionary<String, Object> props)
        {
            if (array.Count > 0 || !props.ContainsKey("children"))
            {
                props["children"] = arguments[2];
            }
        }

        return obj;
    }

    public static WrapperObject Expose(this Type type) => WrapperObject.CreateFor(type);

    public static String FindName(this IEnumerable<String> names, MemberInfo member)
    {
        var selector = Container.GetService<INameSelector>(CamelNameSelector.Instance);
        return selector.Select(names, member);
    }

    public static String Where(this String str, Function f)
    {
        var args = new Object[2];
        var result = new Char[str.Length];
        var length = 0;
        var index = 0;

        foreach (var chr in str)
        {
            args[0] = chr.ToString();
            args[1] = index++;

            if (f(args).ToBoolean())
            {
                result[length++] = chr;
            }
        }

        return new String(result, 0, length);
    }

    public static Function WrapFunction(this Delegate func) => WrapFunction(func.Method, func.Target);

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

    public static Function DeclareFunction(this Function f, String[] parameters)
    {
        Meta.Define(f, "parameters", parameters.ToArrayObject());
        return f;
    }

    public static Function WrapFunction(this MethodInfo method, Object target)
    {
        var parameters = method.GetParameters();
        var names = parameters.Select(m => m.Name).ToArray();
        var f = default(Function);

        if (parameters.Length != 1 || parameters[0].ParameterType != typeof(Object[]) || method.ReturnType != typeof(Object))
        {
            f = DeclareFunction(args =>
            {
                var result = Curry.Min(parameters.Length, f, args);

                if (result is null && method.TryMatch(parameters, ref args))
                {
                    result = method.Call(target, args);
                }

                return result;
            }, names);
        }
        else
        {
            f = DeclareFunction((Function)Delegate.CreateDelegate(typeof(Function), target, method), names);
        }

        return f;
    }

    public static Object WrapObject(this Object value)
    {
        if (value is Task task)
        {
            return new Future(task);
        }

        if (value is not null)
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
        if (value is WrapperObject wrapped)
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

    public static Object MatchString(String pattern, String value)
    {
        var rx = new Regex(pattern, RegexOptions.ECMAScript | RegexOptions.CultureInvariant);
        var mc = rx.Matches(value);

        var result = new Dictionary<String, Object>
        {
            { "matches", WrapMatches(mc) },
            { "success", mc.Count > 0 }
        };

        return result;
    }

    private static Object WrapMatches(MatchCollection mc)
    {
        var results = new Dictionary<String, Object>
        {
            { "count", (Double)mc.Count }
        };

        for (var i = 0; i < mc.Count; i++)
        {
            results[i.ToString()] = WrapMatch(mc[i]);
        }

        return results;
    }

    private static Object WrapMatch(Match match)
    {
        var results = new Dictionary<String, Object>
        {
            { "value", match.Value },
            { "index", (Double)match.Index },
            { "count", (Double)match.Groups.Count }
        };

        for (var i = 0; i < match.Groups.Count; i++)
        {
            if (match.Groups[i].Success)
            {
                results[i.ToString()] = match.Groups[i].Value;
            }
        }

        return results;
    }
}
