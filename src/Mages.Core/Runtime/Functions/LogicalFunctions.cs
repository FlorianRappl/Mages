namespace Mages.Core.Runtime.Functions
{
    using System;

    /// <summary>
    /// The collection of all logical functions.
    /// </summary>
    public static class LogicalFunctions
    {
        /// <summary>
        /// Wraps the Double.IsNaN function.
        /// </summary>
        public static readonly Function IsNaN = new LogicalFunction(Double.IsNaN).Invoke;

        /// <summary>
        /// Contains the is integer function.
        /// </summary>
        public static readonly Function IsInt = new LogicalFunction(Logic.IsInteger).Invoke;

        /// <summary>
        /// Contains the is prime function.
        /// </summary>
        public static readonly Function IsPrime = new LogicalFunction(Logic.IsPrime).Invoke;

        /// <summary>
        /// Wraps the Double.IsInfinity function.
        /// </summary>
        public static readonly Function IsInfty = new LogicalFunction(Double.IsInfinity).Invoke;
    }
}
