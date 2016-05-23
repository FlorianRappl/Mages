namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Linq;

    /// <summary>
    /// The collection of all comparison functions.
    /// </summary>
    public static class ComparisonFunctions
    {
        /// <summary>
        /// Wraps the Math.Min function.
        /// </summary>
        public static readonly Function Min = new ReducerFunction(Math.Min).Invoke;

        /// <summary>
        /// Wraps the Math.Max function.
        /// </summary>
        public static readonly Function Max = new ReducerFunction(Math.Max).Invoke;

        /// <summary>
        /// Wraps the Enumerable.OrderBy function.
        /// </summary>
        public static readonly Function Sort = new ArrayFunction(vec => vec.OrderBy(x => x)).Invoke;

        /// <summary>
        /// Wraps the Enumerable.Reverse function.
        /// </summary>
        public static readonly Function Reverse = new ArrayFunction(vec => vec.Reverse()).Invoke;

        /// <summary>
        /// Contains the equality operator.
        /// </summary>
        public static readonly Function Eq = new Function(BinaryOperators.Eq);

        /// <summary>
        /// Contains the not equals operator.
        /// </summary>
        public static readonly Function Neq = new Function(BinaryOperators.Neq);

        /// <summary>
        /// Contains the greater or equals operator.
        /// </summary>
        public static readonly Function Geq = new Function(BinaryOperators.Geq);

        /// <summary>
        /// Contains the greater than operator.
        /// </summary>
        public static readonly Function Gt = new Function(BinaryOperators.Gt);

        /// <summary>
        /// Contains the less or equals operator.
        /// </summary>
        public static readonly Function Leq = new Function(BinaryOperators.Leq);

        /// <summary>
        /// Contains the less than operator.
        /// </summary>
        public static readonly Function Lt = new Function(BinaryOperators.Lt);
    }
}
