namespace Mages.Core.Runtime.Converters;

using System;

sealed class TypeConverter(Type from, Type to, Func<Object, Object> converter, Int32 rating)
{
    private readonly Type _from = from;
    private readonly Type _to = to;
    private readonly Func<Object, Object> _converter = converter;
    private readonly Int32 _rating = rating;

    public static TypeConverter Create<TFrom, TTo>(Func<TFrom, Object> converter, Int32 rating)
    {
        return new TypeConverter(typeof(TFrom), typeof(TTo), x => converter((TFrom)x), rating);
    }

    public Type From => _from;

    public Type To => _to;

    public Func<Object, Object> Converter => _converter;

    public Int32 Rating => _rating;
}
