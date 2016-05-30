namespace Mages.Core.Runtime
{
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

        public static MethodBase Find(this IEnumerable<MethodBase> methods, Object[] arguments, Type[] currentParameters)
        {
            foreach (var method in methods)
            {
                var actualParameters = method.GetParameters();
                var length = actualParameters.Length;
                var values = new Object[length];

                if (currentParameters.Length == length)
                {
                    var i = 0;

                    while (i < length)
                    {
                        var source = currentParameters[i];
                        var target = actualParameters[i].ParameterType;
                        var converter = Helpers.Converters.FindConverter(source, target);
                        values[i] = converter.Invoke(arguments[i]);

                        if (values[i] == null)
                        {
                            break;
                        }

                        i++;
                    }

                    if (i == length)
                    {
                        values.CopyTo(arguments, 0);
                        return method;
                    }
                }
            }

            return null;
        }

        public static Dictionary<String, BaseProxy> GetStaticProxies(this Type type, WrapperObject target)
        {
            var proxies = new Dictionary<String, BaseProxy>();
            var ctors = type.GetConstructors();
            var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
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
            var fields = type.GetFields();
            var properties = type.GetProperties();
            var methods = type.GetMethods();
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
                proxies[name] = new IndexProxy(target, indices.ToArray());
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
    }
}
