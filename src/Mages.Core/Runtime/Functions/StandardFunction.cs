namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Collections.Generic;

    public abstract class StandardFunction
    {
        protected readonly Object NotImplemented = new Object();

        public virtual Object Invoke(Double value)
        {
            return NotImplemented;
        }

        public virtual Object Invoke(Boolean value)
        {
            return Invoke(value ? 1.0 : 0.0);
        }

        public virtual Object Invoke(String value)
        {
            return NotImplemented;
        }

        public virtual Object Invoke(IDictionary<String, Object> obj)
        {
            return NotImplemented;
        }

        public virtual Object Invoke(Double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var result = new Double[cols, rows];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = (Double)Invoke(matrix[i, j]);
                }
            }

            return result;
        }

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

                if (Object.ReferenceEquals(NotImplemented, result))
                {
                    throw new InvalidOperationException("The operation is invalid.");
                }

                return result;
            }

            return new Function(Invoke);
        }
    }
}
