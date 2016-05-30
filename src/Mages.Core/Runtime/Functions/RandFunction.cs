namespace Mages.Core.Runtime.Functions
{
    using System;

    sealed class RandFunction
    {
        [ThreadStatic]
        private static Random _random;

        public Object Invoke(Object[] arguments)
        {
            var rows = 1;
            var cols = 1;

            if (arguments.Length == 1)
            {
                rows = 1;
                cols = ToInteger(arguments[0]);
            }
            else if (arguments.Length == 2)
            {
                rows = ToInteger(arguments[0]);
                cols = ToInteger(arguments[1]);
            }

            if (_random == null)
            {
                _random = new Random();
            }

            rows = Math.Max(1, rows);
            cols = Math.Max(1, cols);

            if (rows != 1 || cols != 1)
            {
                return CreateMatrix(rows, cols);
            }

            return _random.NextDouble();
        }

        private Double[,] CreateMatrix(Int32 rows, Int32 cols)
        {
            var matrix = new Double[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    matrix[i, j] = _random.NextDouble();
                }
            }

            return matrix;
        }

        private static Int32 ToInteger(Object argument)
        {
            var count = argument as Double?;
            return count.HasValue ? (Int32)count.Value : 1;
        }
    }
}
