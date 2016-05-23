namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;

    sealed class TypeConverterMap
    {
        private readonly List<TypeConverter> _converters = new List<TypeConverter>();
        private readonly Func<Object, Object> _default = _ => null;

        public TypeConverterMap()
        {
            _converters.Add(TypeConverter.Create<Double, Single>(x => (Single)x));
            _converters.Add(TypeConverter.Create<Double, Int16>(x => (Int16)x));
            _converters.Add(TypeConverter.Create<Double, UInt16>(x => (UInt16)x));
            _converters.Add(TypeConverter.Create<Double, Int32>(x => (Int32)x));
            _converters.Add(TypeConverter.Create<Double, UInt32>(x => (UInt32)x));
            _converters.Add(TypeConverter.Create<Double, Int64>(x => (Int64)x));
            _converters.Add(TypeConverter.Create<Double, UInt64>(x => (UInt64)x));
            _converters.Add(TypeConverter.Create<Double, Boolean>(x => x.ToBoolean()));
            _converters.Add(TypeConverter.Create<Double, String>(x => Stringify.This(x)));
            _converters.Add(TypeConverter.Create<Double, Double[,]>(x => x.ToMatrix()));

            _converters.Add(TypeConverter.Create<String, Double>(x => x.ToNumber()));
            _converters.Add(TypeConverter.Create<String, Boolean>(x => x.ToBoolean()));

            _converters.Add(TypeConverter.Create<Boolean, Double>(x => x.ToNumber()));
            _converters.Add(TypeConverter.Create<Boolean, String>(x => Stringify.This(x)));
            _converters.Add(TypeConverter.Create<Boolean, Double[,]>(x => x.ToMatrix()));

            _converters.Add(TypeConverter.Create<Double[,], Boolean>(x => x.ToBoolean()));
            _converters.Add(TypeConverter.Create<Double[,], Double>(x => x.ToNumber()));

            _converters.Add(TypeConverter.Create<IDictionary<String, Object>, String>(x => Stringify.This(x)));
            _converters.Add(TypeConverter.Create<IDictionary<String, Object>, Boolean>(x => x.ToBoolean()));

            _converters.Add(TypeConverter.Create<Single, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Int16, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<UInt16, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Int32, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<UInt32, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Int64, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<UInt64, Double>(x => (Double)x));
        }

        public Func<Object, Object> FindConverter(Type from, Type to)
        {
            var length = _converters.Count;

            for (var i = 0; i < length; ++i)
            {
                var converter = _converters[i];

                if (converter.From == from && converter.To == to)
                {
                    return converter.Converter;
                }
            }

            return _default;
        }
    }
}
