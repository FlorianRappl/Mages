namespace Mages.Core.Runtime.Functions
{
    using Mages.Core.Runtime.Converters;
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

        /// <summary>
        /// Contains the any function.
        /// </summary>
        public static readonly Function Any = new Function(args =>
        {
            if (args.Length > 0)
            {
                var matrix = args[0] as Double[,];
                return matrix != null ? matrix.AnyTrue() : args[0].ToBoolean();
            }

            return false;
        });

        /// <summary>
        /// Contains the all function.
        /// </summary>
        public static readonly Function All = new Function(args =>
        {
            if (args.Length > 0)
            {
                var matrix = args[0] as Double[,];
                return matrix != null ? matrix.AllTrue() : args[0].ToBoolean();
            }

            return false;
        });
    }
}
