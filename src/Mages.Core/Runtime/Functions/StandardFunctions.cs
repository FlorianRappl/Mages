namespace Mages.Core.Runtime.Functions
{
    using System;

    /// <summary>
    /// The collection of all standard functions.
    /// </summary>
    public static class StandardFunctions
    {
        public static readonly Function Sqrt = (new ArithmeticFunction(x => Math.Sqrt(x))).Invoke;

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

        public static readonly Function Add = new Function(BinaryOperators.Add);

        public static readonly Function And = new Function(BinaryOperators.And);

        public static readonly Function Eq = new Function(BinaryOperators.Eq);

        public static readonly Function Geq = new Function(BinaryOperators.Geq);

        public static readonly Function Gt = new Function(BinaryOperators.Gt);

        public static readonly Function LDiv = new Function(BinaryOperators.LDiv);

        public static readonly Function Leq = new Function(BinaryOperators.Leq);

        public static readonly Function Lt = new Function(BinaryOperators.Lt);

        public static readonly Function Mod = new Function(BinaryOperators.Mod);

        public static readonly Function Mul = new Function(BinaryOperators.Mul);

        public static readonly Function Neq = new Function(BinaryOperators.Neq);

        public static readonly Function Or = new Function(BinaryOperators.Or);

        public static readonly Function Pow = new Function(BinaryOperators.Pow);

        public static readonly Function RDiv = new Function(BinaryOperators.RDiv);

        public static readonly Function Sub = new Function(BinaryOperators.Sub);

        public static readonly Function Rand = (new RandFunction()).Invoke;
    }
}
