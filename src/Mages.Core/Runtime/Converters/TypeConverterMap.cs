namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;

    sealed class TypeConverterMap
    {
        private readonly List<TypeConverter> _converters = new List<TypeConverter>();
        private readonly Dictionary<Type, Dictionary<Type, Func<Object, Object>>> _cache = new Dictionary<Type, Dictionary<Type, Func<Object, Object>>>();
        private readonly Func<Object, Object> _default = _ => _ != null ? _ as IDictionary<String, Object> ?? new WrapperObject(_) : _;
        private readonly Func<Object, Object> _identity = _ => _;

        public TypeConverterMap()
        {
            _converters.Add(TypeConverter.Create<Double, Single>(x => (Single)x));
            _converters.Add(TypeConverter.Create<Double, Decimal>(x => (Decimal)x));
            _converters.Add(TypeConverter.Create<Double, Byte>(x => (Byte)Math.Max(0, Math.Min(255, x))));
            _converters.Add(TypeConverter.Create<Double, Int16>(x => (Int16)x));
            _converters.Add(TypeConverter.Create<Double, UInt16>(x => (UInt16)Math.Max(0, x)));
            _converters.Add(TypeConverter.Create<Double, Int32>(x => (Int32)x));
            _converters.Add(TypeConverter.Create<Double, UInt32>(x => (UInt32)Math.Max(0, x)));
            _converters.Add(TypeConverter.Create<Double, Int64>(x => (Int64)x));
            _converters.Add(TypeConverter.Create<Double, UInt64>(x => (UInt64)Math.Max(0, x)));
            _converters.Add(TypeConverter.Create<Double, Boolean>(x => x.ToBoolean()));
            _converters.Add(TypeConverter.Create<Double, String>(x => Stringify.This(x)));
            _converters.Add(TypeConverter.Create<Double, Double[,]>(x => x.ToMatrix()));

            _converters.Add(TypeConverter.Create<String, Double>(x => x.ToNumber()));
            _converters.Add(TypeConverter.Create<String, Boolean>(x => x.ToBoolean()));
            _converters.Add(TypeConverter.Create<String, Char>(x => x.Length > 0 ? x[0] : Char.MinValue));

            _converters.Add(TypeConverter.Create<Boolean, Double>(x => x.ToNumber()));
            _converters.Add(TypeConverter.Create<Boolean, String>(x => Stringify.This(x)));
            _converters.Add(TypeConverter.Create<Boolean, Double[,]>(x => x.ToMatrix()));

            _converters.Add(TypeConverter.Create<Double[,], Boolean>(x => x.ToBoolean()));
            _converters.Add(TypeConverter.Create<Double[,], Double>(x => x.ToNumber()));
            _converters.Add(TypeConverter.Create<Double[,], Double[]>(x => x.ToVector()));
            _converters.Add(TypeConverter.Create<Double[,], List<Double>>(x => x.ToList()));

            _converters.Add(TypeConverter.Create<IDictionary<String, Object>, String>(x => Stringify.This(x)));
            _converters.Add(TypeConverter.Create<IDictionary<String, Object>, Boolean>(x => x.ToBoolean()));

            _converters.Add(TypeConverter.Create<Single, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Int16, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<UInt16, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Int32, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<UInt32, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Int64, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<UInt64, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Decimal, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Byte, Double>(x => (Double)x));
            _converters.Add(TypeConverter.Create<Char, String>(x => x.ToString()));
            _converters.Add(TypeConverter.Create<Double[], Double[,]>(x => x.ToMatrix()));
            _converters.Add(TypeConverter.Create<List<Double>, Double[,]>(x => x.ToMatrix()));
        }

        public Func<Object, Object> FindConverter(Type to)
        {
            var mapping = GetConverterMappingFromCache(to);

            return obj =>
            {
                if (obj != null)
                {
                    var converter = default(Func<Object, Object>);
                    var type = obj.GetType();
                    
                    if (mapping.TryGetValue(type, out converter))
                    {
                        return converter.Invoke(obj);
                    }
                }

                return obj;
            };
        }

        public Func<Object, Object> FindConverter(Type from, Type to)
        {
            if (from != to)
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

            return _identity;
        }

        private Dictionary<Type, Func<Object, Object>> GetConverterMappingFromCache(Type to)
        {
            var mapping = default(Dictionary<Type, Func<Object, Object>>);

            if (!_cache.TryGetValue(to, out mapping))
            {
                var length = _converters.Count;
                mapping = new Dictionary<Type, Func<Object, Object>>();

                for (var i = 0; i < length; ++i)
                {
                    var converter = _converters[i];

                    if (converter.To == to)
                    {
                        mapping.Add(converter.From, converter.Converter);
                    }
                }

                _cache.Add(to, mapping);
            }

            return mapping;
        }
    }
}
