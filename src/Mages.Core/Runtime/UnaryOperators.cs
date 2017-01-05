namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;

    static class UnaryOperators
    {
        public static Object Not(Object[] args)
        {
            return If.Is<Double[,]>(args, x => x.AreEqual(0.0)) ??
                If.Is<Object>(args, x => !x.ToBoolean()) ??
                true;
        }

        public static Object Positive(Object[] args)
        {
            return If.Is<Double[,]>(args, x => x) ??
                If.Is<Object>(args, x => x.ToNumber()) ??
                Double.NaN;
        }

        public static Object Negative(Object[] args)
        {
            return If.Is<Double[,]>(args, x => x.ForEach(y => -y)) ??
                If.Is<Object>(args, x => -x.ToNumber()) ??
                Double.NaN;
        }

        public static Object Factorial(Object[] args)
        {
            return If.Is<Double[,]>(args, x => x.ForEach(y => Mathx.Factorial(y))) ??
                If.Is<Object>(args, x => Mathx.Factorial(x.ToNumber())) ??
                Double.NaN;
        }

        public static Object Transpose(Object[] args)
        {
            return If.Is<Double[,]>(args, x => x.Transpose()) ??
                args[0].ToNumber().ToMatrix();
        }

        public static Object Abs(Object[] args)
        {
            return If.Is<Double[,]>(args, x => Matrix.Abs(x)) ??
                Math.Abs(args[0].ToNumber());
        }

        public static Object Type(Object[] args)
        {
            return args[0].ToType();
        }
    }
}
