namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;

    static class UnaryOperators
    {
        public static Object Not(Object[] args)
        {
            return Curry.MinOne(StandardOperators.Not, args) ??
                If.Is<Double[,]>(args, x => x.AreEqual(0.0)) ??
                If.Is<Object>(args, x => !x.ToBoolean()) ??
                true;
        }

        public static Object Positive(Object[] args)
        {
            return Curry.MinOne(StandardOperators.Positive, args) ??
                If.Is<Double[,]>(args, x => x) ??
                If.Is<Object>(args, x => x.ToNumber()) ??
                Double.NaN;
        }

        public static Object Negative(Object[] args)
        {
            return Curry.MinOne(StandardOperators.Negative, args) ??
                If.Is<Double[,]>(args, x => x.ForEach(y => -y)) ??
                If.Is<Object>(args, x => -x.ToNumber()) ??
                Double.NaN;
        }

        public static Object Factorial(Object[] args)
        {
            return Curry.MinOne(StandardOperators.Factorial, args) ??
                If.Is<Double[,]>(args, x => x.ForEach(y => y.Factorial())) ??
                If.Is<Object>(args, x => x.ToNumber().Factorial()) ??
                Double.NaN;
        }

        public static Object Transpose(Object[] args)
        {
            return Curry.MinOne(StandardOperators.Transpose, args) ??
                If.Is<Double[,]>(args, x => x.Transpose()) ??
                args[0].ToNumber().ToMatrix();
        }

        public static Object Abs(Object[] args)
        {
            return Curry.MinOne(StandardOperators.Abs, args) ??
                If.Is<Double[,]>(args, x => Matrix.Abs(x)) ??
                Math.Abs(args[0].ToNumber());
        }

        public static Object Type(Object[] args)
        {
            return Curry.MinOne(StandardOperators.Type, args) ?? args[0].ToType();
        }
    }
}
