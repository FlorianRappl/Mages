namespace Mages.Core.Runtime
{
    using System;
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
    }
}
