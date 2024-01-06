namespace Mages.Core.Runtime.Converters
{
    using System;

    sealed class TypeConverter
    {
        private readonly Type _from;
        private readonly Type _to;
        private readonly Func<Object, Object> _converter;
        private readonly Int32 _rating;

        public TypeConverter(Type from, Type to, Func<Object, Object> converter, Int32 rating)
        {
            _from = from;
            _to = to;
            _converter = converter;
            _rating = rating;
        }

        public static TypeConverter Create<TFrom, TTo>(Func<TFrom, Object> converter, Int32 rating)
        {
            return new TypeConverter(typeof(TFrom), typeof(TTo), x => converter((TFrom)x), rating);
        }

        public Type From => _from;

        public Type To => _to;

        public Func<Object, Object> Converter => _converter;

        public Int32 Rating => _rating;
    }
}
