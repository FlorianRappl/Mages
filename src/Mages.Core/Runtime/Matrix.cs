namespace Mages.Core.Runtime
{
    using System;

    static class Matrix
    {
        public static Int32 GetRows(this Double[,] matrix)
        {
            return matrix.GetLength(0);
        }

        public static Int32 GetColumns(this Double[,] matrix)
        {
            return matrix.GetLength(1);
        }

        public static void SetValue(this Double[,] matrix, Int32 i, Int32 j, Double value)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();

            if (i >= 0 && i < rows && j >= 0 && j < cols)
            {
                matrix[i, j] = value;
            }
        }

        public static Boolean Fits(this Double[,] a, Double[,] b)
        {
            var rowsA = a.GetRows();
            var rowsB = b.GetRows();
            var colsA = a.GetColumns();
            var colsB = b.GetColumns();

            return rowsA == rowsB && colsA == colsB;
        }

        public static Double[,] Add(this Double[,] a, Double[,] b)
        {
            if (a.Fits(b))
            {
                var rows = a.GetRows();
                var cols = a.GetColumns();
                var result = new Double[rows, cols];

                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < cols; j++)
                    {
                        result[i, j] = a[i, j] + b[i, j];
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] Subtract(this Double[,] a, Double[,] b)
        {
            if (a.Fits(b))
            {
                var rows = a.GetRows();
                var cols = a.GetColumns();
                var result = new Double[rows, cols];

                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < cols; j++)
                    {
                        result[i, j] = a[i, j] - b[i, j];
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] Multiply(this Double[,] a, Double[,] b)
        {
            var rows = a.GetRows();
            var cols = b.GetColumns();
            var length = a.GetColumns();

            if (length == b.GetRows())
            {
                var result = new Double[rows, cols];

                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < cols; j++)
                    {
                        var value = 0.0;

                        for (var k = 0; k < length; k++)
                        {
                            value += a[i, k] * b[k, j];
                        }

                        result[i, j] = value;
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] Multiply(this Double[,] a, Double b)
        {
            var rows = a.GetRows();
            var cols = a.GetColumns();
            var result = new Double[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = a[i, j] * b;
                }
            }

            return result;
        }

        public static Double[,] Transpose(this Double[,] matrix)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
            var result = new Double[cols, rows];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        public static Double GetValue(this Double[,] matrix, Int32 i, Int32 j)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();

            if (i >= 0 && i < rows && j >= 0 && j < cols)
            {
                return matrix[i, j];
            }

            return 0.0;
        }
    }
}
