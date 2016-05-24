namespace Mages.Core.Runtime
{
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
            return Binary<Double, Double>(args, (x, y) => x / y);
        }

        public static Object LDiv(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => y / x);
        }

        public static Object Pow(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => Math.Pow(x, y));
        }

        public static Object Mod(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x % y);
        }

        public static Object And(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x != 0.0 && y != 0.0 ? 1.0 : 0.0);
        }

        public static Object Or(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x != 0.0 || y != 0.0 ? 1.0 : 0.0);
        }

        public static Object Eq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x == y ? 1.0 : 0.0);
        }

        public static Object Neq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x != y ? 1.0 : 0.0);
        }

        public static Object Gt(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x > y ? 1.0 : 0.0);
        }

        public static Object Geq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x >= y ? 1.0 : 0.0);
        }

        public static Object Lt(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x < y ? 1.0 : 0.0);
        }

        public static Object Leq(Object[] args)
        {
            return Binary<Double, Double>(args, (x, y) => x <= y ? 1.0 : 0.0);
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
