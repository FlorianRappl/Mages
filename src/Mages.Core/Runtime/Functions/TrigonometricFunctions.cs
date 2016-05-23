namespace Mages.Core.Runtime.Functions
{
    using System;

    /// <summary>
    /// The collection of all standard functions.
    /// </summary>
    public static class TrigonometricFunctions
    {
        /// <summary>
        /// Wraps the Math.Sin function.
        /// </summary>
        public static readonly Function Sin = new ArithmeticFunction(Math.Sin).Invoke;

        /// <summary>
        /// Wraps the Math.Cos function.
        /// </summary>
        public static readonly Function Cos = new ArithmeticFunction(Math.Cos).Invoke;

        /// <summary>
        /// Wraps the Math.Tan function.
        /// </summary>
        public static readonly Function Tan = new ArithmeticFunction(Math.Tan).Invoke;

        /// <summary>
        /// Wraps the Math.Sinh function.
        /// </summary>
        public static readonly Function Sinh = new ArithmeticFunction(Math.Sinh).Invoke;

        /// <summary>
        /// Wraps the Math.Cosh function.
        /// </summary>
        public static readonly Function Cosh = new ArithmeticFunction(Math.Cosh).Invoke;

        /// <summary>
        /// Wraps the Math.Tanh function.
        /// </summary>
        public static readonly Function Tanh = new ArithmeticFunction(Math.Tanh).Invoke;

        /// <summary>
        /// Wraps the Math.Asin function.
        /// </summary>
        public static readonly Function ArcSin = new ArithmeticFunction(Math.Asin).Invoke;

        /// <summary>
        /// Wraps the Math.Acos function.
        /// </summary>
        public static readonly Function ArcCos = new ArithmeticFunction(Math.Acos).Invoke;

        /// <summary>
        /// Wraps the Math.Atan function.
        /// </summary>
        public static readonly Function ArcTan = new ArithmeticFunction(Math.Atan).Invoke;
    }
}
