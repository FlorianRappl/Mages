namespace Mages.Core.Runtime.Functions
{
    using Mages.Core.Runtime.Converters;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a template for a flexible function.
    /// </summary>
    public abstract class StandardFunction
    {
        /// <summary>
        /// Defines the placeholder to return for indicating a not implemented function.
        /// </summary>
        protected readonly Object NotImplemented = new Object();

        /// <summary>
        /// Invokes the function with a single number argument.
        /// </summary>
        /// <param name="value">The number.</param>
        /// <returns>The return value.</returns>
        public virtual Object Invoke(Double value)
        {
            return NotImplemented;
        }

        /// <summary>
        /// Invokes the function with a single boolean argument.
        /// </summary>
        /// <param name="value">The boolean.</param>
        /// <returns>The return value.</returns>
        public virtual Object Invoke(Boolean value)
        {
            return Invoke(value.ToNumber());
        }

        /// <summary>
        /// Invokes the function with a single string argument.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>The return value.</returns>
        public virtual Object Invoke(String value)
        {
            return NotImplemented;
        }

        /// <summary>
        /// Invokes the function with a single function argument.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <returns>The return value.</returns>
        public virtual Object Invoke(Function function)
        {
            return NotImplemented;
        }

        /// <summary>
        /// Invokes the function with a single object argument.
        /// </summary>
        /// <param name="obj">The dictionary.</param>
        /// <returns>The return value.</returns>
        public virtual Object Invoke(IDictionary<String, Object> obj)
        {
            return NotImplemented;
        }

        /// <summary>
        /// Invokes the function with a single matrix argument.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The return value.</returns>
        public virtual Object Invoke(Double[,] matrix)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
            var result = new Double[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = Compute(matrix[i, j]);
                }
            }

            return result;
        }

        /// <summary>
        /// Invokes the function with any number of arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The return value.</returns>
        public Object Invoke(Object[] arguments)
        {
            if (arguments.Length > 0)
            {
                var argument = arguments[0];
                var result = NotImplemented;

                if (argument is Double)
                {
                    result = Invoke((Double)argument);
                }
                else if (argument is Boolean)
                {
                    result = Invoke((Boolean)argument);
                }
                else if (argument is String)
                {
                    result = Invoke((String)argument);
                }
                else if (argument is IDictionary<String, Object>)
                {
                    result = Invoke((IDictionary<String, Object>)argument);
                }
                else if (argument is Double[,])
                {
                    result = Invoke((Double[,])argument);
                }
                else if (argument is Function)
                {
                    result = Invoke((Function)argument);
                }

                if (Object.ReferenceEquals(NotImplemented, result))
                {
                    throw new InvalidOperationException("The operation is invalid.");
                }

                return result;
            }

            return new Function(Invoke);
        }

        /// <summary>
        /// Computes the value for a single element.
        /// </summary>
        /// <param name="value">The value input.</param>
        /// <returns>The result.</returns>
        protected virtual Double Compute(Double value)
        {
            return Double.NaN;
        }
    }
}
