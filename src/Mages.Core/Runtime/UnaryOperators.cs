namespace Mages.Core.Runtime
{
    using System;

    public static class UnaryOperators
    {
        public static Object Not(Object[] args)
        {
            return Logic.IsFalse((Double)args[0]) ? 1.0 : 0.0;
        }

        public static Object Positive(Object[] args)
        {
            return +(Double)args[0];
        }

        public static Object Negative(Object[] args)
        {
            return -(Double)args[0];
        }

        public static Object Factorial(Object[] args)
        {
            var value = (Double)args[0];
            return value.Factorial();
        }

        public static Object Transpose(Object[] args)
        {
            var matrix = (Double[,])args[0];
            return matrix.Transpose();
        }
    }
}
