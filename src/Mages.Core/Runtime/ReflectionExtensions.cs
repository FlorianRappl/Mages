namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Proxies;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    static class ReflectionExtensions
    {
        public static MethodBase Find(this MethodBase[] methods, Object[] arguments, Type[] currentParameters)
        {
            foreach (var method in methods)
            {
                var actualParameters = method.GetParameters();

                if (currentParameters.Length == actualParameters.Length)
                {
                    var length = actualParameters.Length;
                    var i = 0;

                    while (i < length)
                    {
                        var source = currentParameters[i];
                        var target = actualParameters[i].ParameterType;
                        var converter = Helpers.Converters.FindConverter(source, target);
                        var value = converter.Invoke(arguments[i]);

                        if (value != null)
                        {
                            arguments[i] = value;
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (i == length)
                    {
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

            if (ctors.Length > 0)
            {
                proxies.Add("create", new ConstructorProxy(target, ctors));
            }

            return proxies;
        }

        public static Dictionary<String, BaseProxy> GetMemberProxies(this Type type, WrapperObject target)
        {
            var proxies = new Dictionary<String, BaseProxy>();
            var fields = type.GetFields();
            var properties = type.GetProperties();
            var methods = type.GetMethods();

            foreach (var field in fields)
            {
                proxies.Add(field.Name, new FieldProxy(target, field));
            }

            foreach (var property in properties)
            {
                if (property.GetIndexParameters().Length == 0)
                {
                    proxies.Add(property.Name, new PropertyProxy(target, property));
                }
            }

            foreach (var method in methods.Where(m => !m.IsSpecialName).GroupBy(m => m.Name))
            {
                var overloads = method.ToArray();
                proxies.Add(method.Key, new MethodProxy(target, overloads));
            }

            return proxies;
        }
    }
}
