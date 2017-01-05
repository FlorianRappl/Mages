namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;

    static class BinaryOperators
    {
        public static Object Add(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x + y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Add(y)) ??
                If.Is<String, String>(args, (y, x) => String.Concat(x, y)) ??
                If.Is<Object, String>(args, (y, x) => String.Concat(x, Stringify.This(y))) ??
                If.Is<String, Object>(args, (y, x) => String.Concat(Stringify.This(x), y)) ??
                If.IsNotNull(args, m => m.ToNumber(), (y, x) => x + y);
        }

        public static Object Sub(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x - y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Subtract(y)) ??
                If.IsNotNull(args, m => m.ToNumber(), (y, x) => x - y);
        }
        
        public static Object Mul(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x * y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Multiply(y)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Multiply(y)) ??
                If.Is<Double[,], Double>(args, Matrix.Multiply) ??
                If.IsNotNull(args, m => m.ToNumber(), (y, x) => x * y);
        }

        public static Object RDiv(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x / y) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Divide(y)) ??
                If.IsNotNull(args, m => m.ToNumber(), (y, x) => x / y);
        }

        public static Object LDiv(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => y / x) ??
                If.Is<Double[,], Double>(args, (y, x) => y.Divide(x)) ??
                If.IsNotNull(args, m => m.ToNumber(), (y, x) => y / x);
        }

        public static Object Pow(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => Math.Pow(x, y)) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Pow(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => x.Pow(y)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Pow(y)) ??
                If.IsNotNull(args, m => m.ToNumber(), (y, x) => Math.Pow(x, y));
        }

        public static Object Mod(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x % y) ??
                If.IsNotNull(args, m => m.ToNumber(), (y, x) => x % y);
        }

        public static Object And(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x.ToBoolean() && y.ToBoolean()) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x && y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.And(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.And(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.And(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.And(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.And(y.ToNumber())) ??
                (args[1].ToBoolean() && args[0].ToBoolean());
        }

        public static Object Or(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x.ToBoolean() || y.ToBoolean()) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x || y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Or(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.Or(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Or(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.Or(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.Or(y.ToNumber())) ??
                (args[1].ToBoolean() || args[0].ToBoolean());
        }

        public static Object Eq(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x == y) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x == y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.AreEqual(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.AreEqual(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreEqual(y.ToNumber())) ??
                If.Is<String, String>(args, (y, x) => y.Equals(x)) ??
                Object.ReferenceEquals(args[1], args[0]);
        }

        public static Object Neq(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x != y) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x != y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.AreNotEqual(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.AreNotEqual(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreNotEqual(y.ToNumber())) ??
                If.Is<String, String>(args, (y, x) => !x.Equals(y)) ??
                !Object.ReferenceEquals(args[1], args[0]);
        }

        public static Object Gt(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x > y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterThan(y)) ??
                (args[1].ToNumber() > args[0].ToNumber());
        }

        public static Object Geq(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x >= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterOrEqual(y)) ??
                (args[1].ToNumber() >= args[0].ToNumber());
        }

        public static Object Lt(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x < y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessThan(y)) ??
                (args[1].ToNumber() < args[0].ToNumber());
        }

        public static Object Leq(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x <= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessOrEqual(y)) ??
                (args[1].ToNumber() <= args[0].ToNumber());
        }

        public static Object Pipe(Object[] args)
        {
            return If.Is<Function>(args, f => f.Invoke(new[] { args[1] }));
        }
    }
}
