namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
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
        
        public static Object Getter(this Double[,] matrix, Object[] arguments)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
            var n = 0;
            var i = -1;
            var j = -1;

            if (arguments.Length == 1 && arguments[0].TryGetIndex(out n))
            {
                j = n % cols;
                i = n / cols;
            }
            else if (arguments.Length == 2 && arguments[0].TryGetIndex(out i) && arguments[1].TryGetIndex(out j))
            {
            }

            if (i >= 0 && j >= 0 && i < rows && j < cols)
            {
                return matrix[i, j];
            }

            return null;
        }

        public static Double Abs(Double[,] matrix)
        {
            var sum = 0.0;
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    sum += matrix[i ,j] * matrix[i, j];
                }
            }

            return Math.Sqrt(sum);
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

        public static Double[,] Divide(this Double[,] a, Double b)
        {
            return a.Multiply(1.0 / b);
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

        public static Boolean IsSquare(this Double[,] matrix)
        {
            return matrix.GetColumns() == matrix.GetRows();
        }

        public static Double[,] Fill(this Double[,] matrix, Double value)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
            var result = new Double[rows, cols];
            var length = Math.Min(rows, cols);

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = value;
                }
            }

            return result;
        }

        public static Double[,] Identity(this Double[,] matrix)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
            var result = new Double[rows, cols];
            var length = Math.Min(rows, cols);

            for (var i = 0; i < length; i++)
            {
                result[i, i] = 1.0;
            }

            return result;
        }

        public static Double[,] Pow(this Double[,] matrix, Double value)
        {
            if (value.IsInteger() && matrix.IsSquare())
            {
                var n = (Int32)value;
                var result = matrix.Identity();

                while (n-- > 0)
                {
                    result = result.Multiply(matrix);
                }

                return result;
            }

            return null;
        }

        public static Double[,] Pow(this Double value, Double[,] matrix)
        {
            var result = matrix.Fill(value);
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = Math.Pow(result[i, j], matrix[i, j]);
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

        public static void SetValue(this Double[,] matrix, Int32 i, Int32 j, Double value)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();

            if (i >= 0 && i < rows && j >= 0 && j < cols)
            {
                matrix[i, j] = value;
            }
        }

        public static Double[,] And(this Double[,] a, Double b)
        {
            return a.And(a.Fill(b));
        }

        public static Double[,] And(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j].ToBoolean() && b[i, j].ToBoolean()).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] IsGreaterThan(this Double[,] a, Double b)
        {
            return a.IsGreaterThan(a.Fill(b));
        }

        public static Double[,] IsGreaterThan(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j] > b[i, j]).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] IsGreaterOrEqual(this Double[,] a, Double b)
        {
            return a.IsGreaterOrEqual(a.Fill(b));
        }

        public static Double[,] IsGreaterOrEqual(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j] >= b[i, j]).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] AreNotEqual(this Double[,] a, Double b)
        {
            return a.AreNotEqual(a.Fill(b));
        }

        public static Double[,] AreNotEqual(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j] != b[i, j]).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] IsLessThan(this Double[,] a, Double b)
        {
            return a.IsLessThan(a.Fill(b));
        }

        public static Double[,] IsLessThan(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j] < b[i, j]).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] IsLessOrEqual(this Double[,] a, Double b)
        {
            return a.IsLessOrEqual(a.Fill(b));
        }

        public static Double[,] IsLessOrEqual(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j] <= b[i, j]).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] AreEqual(this Double[,] a, Double b)
        {
            return a.AreEqual(a.Fill(b));
        }

        public static Double[,] AreEqual(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j] == b[i, j]).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }

        public static Double[,] Or(this Double[,] a, Double b)
        {
            return a.Or(a.Fill(b));
        }

        public static Double[,] Or(this Double[,] a, Double[,] b)
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
                        result[i, j] = (a[i, j].ToBoolean() || b[i, j].ToBoolean()).ToNumber();
                    }
                }

                return result;
            }

            return null;
        }
    }
}
