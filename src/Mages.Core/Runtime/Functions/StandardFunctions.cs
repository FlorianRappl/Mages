namespace Mages.Core.Runtime.Functions
{
    using System;

    /// <summary>
    /// The collection of all standard functions.
    /// </summary>
    public static class StandardFunctions
    {
        /// <summary>
        /// Wraps the Math.Sqrt function.
        /// </summary>
        public static readonly Function Sqrt = (new ArithmeticFunction(Math.Sqrt)).Invoke;

        /// <summary>
        /// Contains the factorial function.
        /// </summary>
        public static readonly Function Factorial = new Function(UnaryOperators.Factorial);

        /// <summary>
        /// Contains the transpose operator.
        /// </summary>
        public static readonly Function Transpose = new Function(UnaryOperators.Transpose);

        /// <summary>
        /// Contains the negation operator.
        /// </summary>
        public static readonly Function Not = new Function(UnaryOperators.Not);

        /// <summary>
        /// Contains the positive operator.
        /// </summary>
        public static readonly Function Positive = new Function(UnaryOperators.Positive);

        /// <summary>
        /// Contains the negative operator.
        /// </summary>
        public static readonly Function Negative = new Function(UnaryOperators.Negative);

        /// <summary>
        /// Wraps the Math.Abs function.
        /// </summary>
        public static readonly Function Abs = new Function(UnaryOperators.Abs);

        /// <summary>
        /// Wraps the Math.Sign function.
        /// </summary>
        public static readonly Function Sign = (new ArithmeticFunction(x => Math.Sign(x))).Invoke;

        /// <summary>
        /// Wraps the Math.Ceiling function.
        /// </summary>
        public static readonly Function Ceil = (new ArithmeticFunction(Math.Ceiling)).Invoke;

        /// <summary>
        /// Wraps the Math.Floor function.
        /// </summary>
        public static readonly Function Floor = (new ArithmeticFunction(Math.Floor)).Invoke;

        /// <summary>
        /// Wraps the Math.Round function.
        /// </summary>
        public static readonly Function Round = (new ArithmeticFunction(Math.Round)).Invoke;

        /// <summary>
        /// Wraps the Math.Exp function.
        /// </summary>
        public static readonly Function Exp = (new ArithmeticFunction(Math.Exp)).Invoke;

        /// <summary>
        /// Wraps the Math.Log function.
        /// </summary>
        public static readonly Function Log = (new ArithmeticFunction(Math.Log)).Invoke;

        /// <summary>
        /// Contains the add operator.
        /// </summary>
        public static readonly Function Add = new Function(BinaryOperators.Add);

        /// <summary>
        /// Contains the and operator.
        /// </summary>
        public static readonly Function And = new Function(BinaryOperators.And);

        /// <summary>
        /// Contains the left division operator.
        /// </summary>
        public static readonly Function LDiv = new Function(BinaryOperators.LDiv);

        /// <summary>
        /// Contains the modulo operator.
        /// </summary>
        public static readonly Function Mod = new Function(BinaryOperators.Mod);

        /// <summary>
        /// Contains the multiplication operator.
        /// </summary>
        public static readonly Function Mul = new Function(BinaryOperators.Mul);

        /// <summary>
        /// Contains the or operator.
        /// </summary>
        public static readonly Function Or = new Function(BinaryOperators.Or);

        /// <summary>
        /// Contains the power operator.
        /// </summary>
        public static readonly Function Pow = new Function(BinaryOperators.Pow);

        /// <summary>
        /// Contains the right division operator.
        /// </summary>
        public static readonly Function RDiv = new Function(BinaryOperators.RDiv);

        /// <summary>
        /// Contains the subtraction operator.
        /// </summary>
        public static readonly Function Sub = new Function(BinaryOperators.Sub);

        /// <summary>
        /// Wraps the random function.
        /// </summary>
        public static readonly Function Rand = (new RandFunction()).Invoke;

        /// <summary>
        /// Contains the throw function.
        /// </summary>
        public static readonly Function Throw = (new ThrowFunction()).Invoke;

        /// <summary>
        /// Contains the catch function.
        /// </summary>
        public static readonly Function Catch = (new CatchFunction()).Invoke;
    }
}
