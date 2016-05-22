namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Functions;
    using System;

    public static class StandardFunctions
    {
        public static readonly Function Sqrt = (new ArithmeticFunction(x => Math.Sqrt(x))).Invoke;

        public static readonly Function Pow = new Function(BinaryOperators.Pow);

        public static readonly Function Factorial = new Function(UnaryOperators.Factorial);

        public static readonly Function Transpose = new Function(UnaryOperators.Transpose);

        public static readonly Function Not = new Function(UnaryOperators.Not);

        public static readonly Function Positive = new Function(UnaryOperators.Positive);

        public static readonly Function Negative = new Function(UnaryOperators.Negative);

        public static readonly Function Abs = new Function(UnaryOperators.Abs);

        public static readonly Function Sign = (new ArithmeticFunction(x => Math.Sign(x))).Invoke;

        public static readonly Function Ceil = (new ArithmeticFunction(x => Math.Ceiling(x))).Invoke;

        public static readonly Function Floor = (new ArithmeticFunction(x => Math.Floor(x))).Invoke;

        public static readonly Function Exp = (new ArithmeticFunction(x => Math.Exp(x))).Invoke;

        public static readonly Function Log = (new ArithmeticFunction(x => Math.Log(x))).Invoke;
    }
}
