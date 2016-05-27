namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using System;

    static class UnaryOperators
    {
        public static Object Not(Object[] args)
        {
            return Unary<Double[,]>(args, x => x.AreEqual(0.0)) ??
                Unary<Object>(args, x => !x.ToBoolean()) ??
                true;
        }

        public static Object Positive(Object[] args)
        {
            return Unary<Double[,]>(args, x => x) ??
                Unary<Object>(args, x => x.ToNumber()) ??
                Double.NaN;
        }

        public static Object Negative(Object[] args)
        {
            return Unary<Double[,]>(args, x => x.ForEach(y => -y)) ??
                Unary<Object>(args, x => -x.ToNumber()) ??
                Double.NaN;
        }

        public static Object Factorial(Object[] args)
        {
            return Unary<Double[,]>(args, x => x.ForEach(y => y.Factorial())) ??
                Unary<Object>(args, x => x.ToNumber().Factorial()) ??
                Double.NaN;
        }

        public static Object Transpose(Object[] args)
        {
            if (args.Length > 0)
            {
                var matrix = args[0] as Double[,];

                if (matrix != null)
                {
                    return matrix.Transpose();
                }

                return args[0].ToNumber().ToMatrix();
            }

            return null;
        }

        public static Object Abs(Object[] args)
        {
            if (args.Length > 0)
            {
                var value = args[0];

                if (value is Double[,])
                {
                    return Matrix.Abs((Double[,])value);
                }

                return Math.Abs(value.ToNumber());
            }

            return null;
        }

        private static Object Unary<T>(Object[] args, Func<T, Object> f)
        {
            if (args[0] is T)
            {
                return f((T)args[0]);
            }

            return null;
        }
    }
}
