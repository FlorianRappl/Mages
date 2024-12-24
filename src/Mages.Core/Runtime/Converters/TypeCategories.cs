namespace Mages.Core.Runtime.Converters;

using System;
using System.Collections.Generic;
using System.Numerics;

static class TypeCategories
{
    public static readonly Dictionary<Type, List<Type>> Mapping = new()
    {
        { typeof(Double), new List<Type> { typeof(Double), typeof(Single), typeof(Decimal), typeof(Byte), typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(Int16), typeof(Int32), typeof(Int64) } },
        { typeof(Complex), new List<Type> { typeof(Complex) } },
        { typeof(Boolean), new List<Type> { typeof(Boolean) } },
        { typeof(String), new List<Type> { typeof(String), typeof(Char) } },
        { typeof(Double[,]), new List<Type> { typeof(Double[,]), typeof(Double[]), typeof(List<Double>) } },
        { typeof(Complex[,]), new List<Type> { typeof(Complex[,]), typeof(Complex[]), typeof(List<Complex>) } },
        { typeof(Function), new List<Type> { typeof(Function), typeof(Delegate) } },
        { typeof(IDictionary<String, Object>), new List<Type> { typeof(IDictionary<String, Object>), typeof(Object) } }
    };

    public static Type FindPrimitive(this Type type)
    {
        foreach (var category in Mapping)
        {
            foreach (var value in category.Value)
            {
                if (value.IsAssignableFrom(type))
                {
                    return category.Key;
                }
            }
        }

        return type;
    }
}
