namespace Mages.Core.Runtime;

using Mages.Core.Runtime.Converters;
using Mages.Core.Runtime.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

static class ReflectionExtensions
{
    public static Object Call(this MethodBase method, Object target, Object[] arguments)
    {
        try
        {
            return method.Invoke(target, arguments).WrapObject();
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    public static Object Call(this ConstructorInfo ctor, WrapperObject obj, Object[] arguments)
    {
        try
        {
            return ctor.Invoke(arguments).WrapObject();
        } 
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    public static Object Call(this MethodBase method, WrapperObject obj, Object[] arguments)
    {
        try
        {
            var target = obj.Content;
            var result = method.Invoke(target, arguments);

            if (Object.ReferenceEquals(result, target))
            {
                return obj;
            }

            return result.WrapObject();
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    public static Int32 MaxParameters(this IEnumerable<MethodBase> methods)
    {
        var mp = 0;

        foreach (var method in methods)
        {
            mp = Math.Max(method.GetParameters().Length, mp);
        }

        return mp;
    }

    public static MethodBase Find(this IEnumerable<MethodBase> methods, Type[] currentParameters, ref Object[] arguments)
    {
        var methodGroups = methods
            .Select(m => new { Info = m, ActualParameters = m.GetParameters() })
            .GroupBy(m => m.ActualParameters.Length)
            .OrderByDescending(m => m.Key);

        foreach (var methodGroup in methodGroups)
        {
            foreach (var method in methodGroup.OrderByDescending(m => GetRating(m.ActualParameters, currentParameters)))
            {
                if (method.Info.TryMatch(method.ActualParameters, currentParameters, ref arguments))
                {
                    return method.Info;
                }
            }
        }

        return null;
    }

    public static Object Convert(this Type source, Object value, Type target)
    {
        if (value is not WrapperObject wrapper)
        {
            var converter = Helpers.Converters.FindConverter(source, target);
            return converter.Invoke(value);
        }
        else if (target.IsInstanceOfType(wrapper.Content))
        {
            return wrapper.Content;
        }

        return null;
    }

    public static Object ConvertParams(this Type[] sources, Object[] arguments, Type target, Int32 offset)
    {
        var rest = sources.Length - offset;
        var bag = Array.CreateInstance(target, rest);

        for (var j = 0; j < rest; j++)
        {
            var i = offset + j;
            var source = sources[i];
            bag.SetValue(source.Convert(arguments[i], target), j);
        }

        return bag;
    }

    public static Boolean TryMatch(this MethodBase method, ParameterInfo[] actualParameters, ref Object[] arguments)
    {
        var currentParameters = arguments.Select(m => m.GetType()).ToArray();
        return method.TryMatch(actualParameters, currentParameters, ref arguments);
    }

    public static Boolean TryMatch(this MethodBase method, ParameterInfo[] actualParameters, Type[] currentParameters, ref Object[] arguments)
    {
        var length = actualParameters.Length;

        if (currentParameters.Length >= length)
        {
            var values = new Object[length];
            var last = length - 1;
            var i = 0;

            while (i < length)
            {
                var source = currentParameters[i];
                var parameter = actualParameters[i];
                var isLast = i == last;
                var value = default(Object);

                if (isLast && TryGetMultiType(parameter, out var element))
                {
                    value = currentParameters.ConvertParams(arguments, element, i);
                }
                else
                {
                    value = source.Convert(arguments[i], parameter.ParameterType);
                }

                if (value == null)
                {
                    break;
                }

                values[i] = value;
                i++;
            }

            if (i == length)
            {
                arguments = values;
                return true;
            }
        }
        else if (currentParameters.Length == length - 1 && actualParameters[length - 1].GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0)
        {
            var extendedPara = new Type[length];
            var extendedArgs = new Object[length];
            arguments.CopyTo(extendedArgs, 0);
            currentParameters.CopyTo(extendedPara, 0);
            arguments = extendedArgs;
            extendedPara[length - 1] = actualParameters[length - 1].ParameterType.GetElementType();
            return method.TryMatch(actualParameters, extendedPara, ref arguments);
        }

        return false;
    }

    public static Dictionary<String, BaseProxy> GetStaticProxies(this Type type, WrapperObject target)
    {
        var proxies = new Dictionary<String, BaseProxy>();
        var flags = BindingFlags.Public | BindingFlags.Static;
        var ctors = type.GetConstructors();
        var fields = type.GetFields(flags);
        var properties = type.GetProperties(flags);
        var methods = type.GetMethods(flags);
        var selector = Container.GetService<INameSelector>(CamelNameSelector.Instance);

        fields.AddToProxy(target, proxies, selector);
        properties.AddToProxy(target, proxies, selector);
        methods.AddToProxy(target, proxies, selector);
        ctors.AddToProxy(target, proxies, selector);

        return proxies;
    }

    public static Dictionary<String, BaseProxy> GetMemberProxies(this Type type, WrapperObject target)
    {
        var proxies = new Dictionary<String, BaseProxy>();
        var flags = BindingFlags.Public | BindingFlags.Instance;
        var fields = type.GetFields(flags);
        var properties = type.GetProperties(flags);
        var methods = type.GetMethods(flags);
        var selector = Container.GetService<INameSelector>(CamelNameSelector.Instance);

        fields.AddToProxy(target, proxies, selector);
        properties.AddToProxy(target, proxies, selector);
        methods.AddToProxy(target, proxies, selector);

        return proxies;
    }

    private static void AddToProxy(this ConstructorInfo[] constructors, WrapperObject target, IDictionary<String, BaseProxy> proxies, INameSelector selector)
    {
        if (constructors.Length > 0)
        {
            var name = selector.Select(proxies.Keys, constructors[0]);
            proxies[name] = new ConstructorProxy(target, constructors);
        }
    }

    private static void AddToProxy(this FieldInfo[] fields, WrapperObject target, IDictionary<String, BaseProxy> proxies, INameSelector selector)
    {
        foreach (var field in fields)
        {
            var name = selector.Select(proxies.Keys, field);
            proxies.Add(name, new FieldProxy(target, field));
        }
    }

    private static void AddToProxy(this PropertyInfo[] properties, WrapperObject target, IDictionary<String, BaseProxy> proxies, INameSelector selector)
    {
        var indices = new List<PropertyInfo>();

        foreach (var property in properties)
        {
            if (property.GetIndexParameters().Length == 0)
            {
                var name = selector.Select(proxies.Keys, property);
                proxies.Add(name, new PropertyProxy(target, property));
            }
            else
            {
                indices.Add(property);
            }
        }

        if (indices.Count > 0)
        {
            var name = selector.Select(proxies.Keys, indices[0]);
            proxies[name] = new IndexProxy(target, [.. indices]);
        }
    }

    private static void AddToProxy(this MethodInfo[] methods, WrapperObject target, IDictionary<String, BaseProxy> proxies, INameSelector selector)
    {
        foreach (var method in methods.Where(m => !m.IsSpecialName).GroupBy(m => m.Name))
        {
            var overloads = method.ToArray();
            var name = selector.Select(proxies.Keys, overloads[0]);
            proxies.Add(name, new MethodProxy(target, overloads));
        }
    }

    private static Int32 GetRating(ParameterInfo[] actualParameters, Type[] currentParameters)
    {
        var rating = 0;
        var apl = actualParameters.Length;
        var cpl = currentParameters.Length;
        var len = Math.Min(apl, cpl);
        var last = len - 1;

        for (var i = 0; i < len; i++)
        {
            var parameter = actualParameters[i];
            var at = parameter.ParameterType;
            var ct = currentParameters[i];
            var isLast = i == last && cpl >= apl;

            if (at == ct)
            {
                rating += 100;
            }
            else if (isLast && TryGetMultiType(parameter, out var ait))
            {
                for (var j = i; j < cpl; j++)
                {
                    var cit = currentParameters[j];

                    if (ait == cit)
                    {
                        continue;
                    }

                    var converter = StandardConverters.List.Find(m => m.From == cit && m.To == ait);

                    if (converter == null)
                    {
                        return 0;
                    }
                }

                rating += 100 * (cpl - apl + 1);
            }
            else
            {
                var converter = StandardConverters.List.Find(m => m.From == ct && m.To == at);

                if (converter != null)
                {
                    rating += converter.Rating;
                }
            }
        }

        return rating;
    }

    private static Boolean TryGetMultiType(ParameterInfo parameterInfo, out Type result)
    {
        var type = parameterInfo.ParameterType;
        result = null;

        if (IsEnumerable(type))
        {
            result = type.GetGenericArguments()[0];
            return true;
        }
        else if (IsParams(parameterInfo))
        {
            result = type.GetElementType();
            return true;
        }

        return false;
    }

    private static Boolean IsEnumerable(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }

    private static Boolean IsParams(ParameterInfo parameterInfo)
    {
        return parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;
    }
}
