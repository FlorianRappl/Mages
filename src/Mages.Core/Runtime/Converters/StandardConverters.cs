namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    static class StandardConverters
    {
        public static readonly List<TypeConverter> List =
        [
            TypeConverter.Create<Double, Single>(x => (Single)x, 80),
            TypeConverter.Create<Double, Complex>(x => new Complex(x, 0.0), 95),
            TypeConverter.Create<Double, Decimal>(x => (Decimal)x, 95),
            TypeConverter.Create<Double, Byte>(x => (Byte)Math.Max(0, Math.Min(255, x)), 20),
            TypeConverter.Create<Double, Int16>(x => (Int16)x, 30),
            TypeConverter.Create<Double, UInt16>(x => (UInt16)Math.Max(0, x), 40),
            TypeConverter.Create<Double, Int32>(x => (Int32)x, 50),
            TypeConverter.Create<Double, UInt32>(x => (UInt32)Math.Max(0, x), 50),
            TypeConverter.Create<Double, Int64>(x => (Int64)x, 70),
            TypeConverter.Create<Double, UInt64>(x => (UInt64)Math.Max(0, x), 60),
            TypeConverter.Create<Double, Boolean>(x => x.ToBoolean(), 10),
            TypeConverter.Create<Double, String>(x => Stringify.This(x), 15),
            TypeConverter.Create<Double, Double[,]>(x => x.ToMatrix(), 15),

            TypeConverter.Create<String, Double>(x => x.ToNumber(), 10),
            TypeConverter.Create<String, Complex>(x => new Complex(x.ToNumber(), 0.0), 5),
            TypeConverter.Create<String, Boolean>(x => x.ToBoolean(), 15),
            TypeConverter.Create<String, Char>(x => x.Length > 0 ? x[0] : Char.MinValue, 10),

            TypeConverter.Create<Boolean, Double>(x => x.ToNumber(), 15),
            TypeConverter.Create<Boolean, Complex>(x => new Complex(x.ToNumber(), 0.0), 10),
            TypeConverter.Create<Boolean, String>(x => Stringify.This(x), 15),
            TypeConverter.Create<Boolean, Double[,]>(x => x.ToMatrix(), 15),

            TypeConverter.Create<Double[,], Boolean>(x => x.ToBoolean(), 15),
            TypeConverter.Create<Double[,], Double>(x => x.ToNumber(), 30),
            TypeConverter.Create<Double[,], Double[]>(x => x.ToVector(), 50),
            TypeConverter.Create<Double[,], List<Double>>(x => x.ToList(), 40),

            TypeConverter.Create<IDictionary<String, Object>, String>(x => Stringify.This(x), 15),
            TypeConverter.Create<IDictionary<String, Object>, Boolean>(x => x.ToBoolean(), 10),
            TypeConverter.Create<IDictionary<String, Object>, Double>(x => x.ToNumber(), 5),

            TypeConverter.Create<Single, Double>(x => (Double)x, 95),
            TypeConverter.Create<Int16, Double>(x => (Double)x, 90),
            TypeConverter.Create<UInt16, Double>(x => (Double)x, 90),
            TypeConverter.Create<Int32, Double>(x => (Double)x, 80),
            TypeConverter.Create<UInt32, Double>(x => (Double)x, 80),
            TypeConverter.Create<Int64, Double>(x => (Double)x, 60),
            TypeConverter.Create<UInt64, Double>(x => (Double)x, 60),
            TypeConverter.Create<Decimal, Double>(x => (Double)x, 40),
            TypeConverter.Create<Byte, Double>(x => (Double)x, 90),

            TypeConverter.Create<Single, Complex>(x => new Complex((Double)x, 0.0), 95),
            TypeConverter.Create<Int16, Complex>(x => new Complex((Double)x, 0.0), 90),
            TypeConverter.Create<UInt16, Complex>(x => new Complex((Double)x, 0.0), 90),
            TypeConverter.Create<Int32, Complex>(x => new Complex((Double)x, 0.0), 90),
            TypeConverter.Create<UInt32, Complex>(x => new Complex((Double)x, 0.0), 80),
            TypeConverter.Create<Int64, Complex>(x => new Complex((Double)x, 0.0), 80),
            TypeConverter.Create<UInt64, Complex>(x => new Complex((Double)x, 0.0), 60),
            TypeConverter.Create<Decimal, Complex>(x => new Complex((Double)x, 0.0), 60),
            TypeConverter.Create<Byte, Complex>(x => new Complex((Double)x, 0.0), 90),

            TypeConverter.Create<Char, String>(x => x.ToString(), 90),
            TypeConverter.Create<Double[], Double[,]>(x => x.ToMatrix(), 95),
            TypeConverter.Create<List<Double>, Double[,]>(x => x.ToMatrix(), 95),
            TypeConverter.Create<Delegate, Function>(Helpers.WrapFunction, 60),
            TypeConverter.Create<Array, Dictionary<String, Object>>(Helpers.WrapArray, 40)
        ];

        public static readonly Func<Object, Object> Identity = _ => _;

        public static readonly Func<Object, Object> Default = _ => _ as IDictionary<String, Object> ?? WrapperObject.CreateFor(_);
    }
}
