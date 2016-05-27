namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using System;

    static class BinaryOperators
    {
        public static Object Add(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x + y) ??
                Binary<Double[,], Double[,]>(args, Matrix.Add) ??
                Binary<String, String>(args, String.Concat);
        }

        public static Object Sub(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x - y) ??
                Binary<Double[,], Double[,]>(args, Matrix.Subtract);
        }
        
        public static Object Mul(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x * y) ??
                Binary<Double[,], Double[,]>(args, Matrix.Multiply) ??
                Binary<Double, Double[,]>(args, (y, x) => x.Multiply(y)) ??
                Binary<Double[,], Double>(args, Matrix.Multiply);
        }

        public static Object RDiv(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x / y) ??
                Binary<Double[,], Double>(args, Matrix.Divide);
        }

        public static Object LDiv(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => y / x) ??
                Binary<Double, Double[,]>(args, (y, x) => x.Divide(y));
        }

        public static Object Pow(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => Math.Pow(x, y)) ??
                Binary<Double[,], Double>(args, Matrix.Pow) ??
                Binary<Double, Double[,]>(args, Matrix.Pow);
        }

        public static Object Mod(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x % y);
        }

        public static Object And(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x.IsTrue() && y.IsTrue()) ??
                Binary<Boolean, Boolean>(args, (x, y) => x && y) ??
                Binary<Double[,], Double[,]>(args, Matrix.And) ??
                Binary<Double[,], Double>(args, Matrix.And) ??
                Binary<Double, Double[,]>(args, (y, x) => x.And(y)) ??
                Binary<Double[,], Boolean>(args, (x, y) => x.And(y.ToNumber())) ??
                Binary<Boolean, Double[,]>(args, (y, x) => x.And(y.ToNumber())) ??
                (args[0].ToBoolean() && args[1].ToBoolean());
        }

        public static Object Or(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x.IsTrue() || y.IsTrue()) ??
                Binary<Boolean, Boolean>(args, (x, y) => x || y) ??
                Binary<Double[,], Double[,]>(args, Matrix.Or) ??
                Binary<Double[,], Double>(args, Matrix.Or) ??
                Binary<Double, Double[,]>(args, (y, x) => x.Or(y)) ??
                Binary<Double[,], Boolean>(args, (x, y) => x.Or(y.ToNumber())) ??
                Binary<Boolean, Double[,]>(args, (y, x) => x.Or(y.ToNumber())) ??
                (args[0].ToBoolean() || args[1].ToBoolean());
        }

        public static Object Eq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x == y) ??
                Binary<Boolean, Boolean>(args, (x, y) => x == y) ??
                Binary<Double[,], Double[,]>(args, Matrix.AreEqual) ??
                Binary<Double[,], Double>(args, Matrix.AreEqual) ??
                Binary<Double, Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                Binary<Double[,], Boolean>(args, (x, y) => x.AreEqual(y.ToNumber())) ??
                Binary<Boolean, Double[,]>(args, (y, x) => x.AreEqual(y.ToNumber())) ??
                Binary<Object, Object>(args, (x, y) => Object.ReferenceEquals(x, y));
        }

        public static Object Neq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x != y) ??
                Binary<Boolean, Boolean>(args, (x, y) => x != y) ??
                Binary<Double[,], Double[,]>(args, Matrix.AreNotEqual) ??
                Binary<Double[,], Double>(args, Matrix.AreNotEqual) ??
                Binary<Double, Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                Binary<Double[,], Boolean>(args, (x, y) => x.AreNotEqual(y.ToNumber())) ??
                Binary<Boolean, Double[,]>(args, (y, x) => x.AreNotEqual(y.ToNumber())) ??
                Binary<Object, Object>(args, (x, y) => !Object.ReferenceEquals(x, y));
        }

        public static Object Gt(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x > y) ??
                Binary<Double[,], Double[,]>(args, Matrix.IsGreaterThan) ??
                Binary<Double[,], Double>(args, Matrix.IsGreaterThan) ??
                Binary<Double, Double[,]>(args, (y, x) => x.IsLessThan(y)) ??
                (args[0].ToNumber() > args[1].ToNumber());
        }

        public static Object Geq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x >= y) ??
                Binary<Double[,], Double[,]>(args, Matrix.IsGreaterOrEqual) ??
                Binary<Double[,], Double>(args, Matrix.IsGreaterOrEqual) ??
                Binary<Double, Double[,]>(args, (y, x) => x.IsLessOrEqual(y)) ??
                (args[0].ToNumber() >= args[1].ToNumber());
        }

        public static Object Lt(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x < y) ??
                Binary<Double[,], Double[,]>(args, Matrix.IsLessThan) ??
                Binary<Double[,], Double>(args, Matrix.IsLessThan) ??
                Binary<Double, Double[,]>(args, (y, x) => x.IsGreaterThan(y)) ??
                (args[0].ToNumber() < args[1].ToNumber());
        }

        public static Object Leq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x <= y) ??
                Binary<Double[,], Double[,]>(args, Matrix.IsLessOrEqual) ??
                Binary<Double[,], Double>(args, Matrix.IsLessOrEqual) ??
                Binary<Double, Double[,]>(args, (y, x) => x.IsGreaterOrEqual(y)) ??
                (args[0].ToNumber() <= args[1].ToNumber());
        }

        private static Object Binary<Tx, Ty>(Object[] args, Func<Tx, Ty, Object> f)
        {
            if (args[0] is Tx && args[1] is Ty)
            {
                return f((Tx)args[0], (Ty)args[1]);
            }

            return null;
        }
    }
}
