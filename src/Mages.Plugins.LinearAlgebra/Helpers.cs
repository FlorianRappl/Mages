namespace Mages.Plugins.LinearAlgebra
{
    using System;

    static class Helpers
    {        
        public static Double Hypot(Double a, Double b)
        {
            var r = 0.0;

            if (Math.Abs(a) > Math.Abs(b))
            {
                r = b / a;
                r = Math.Abs(a) * Math.Sqrt(1 + r * r);
            }
            else if (b != 0)
            {
                r = a / b;
                r = Math.Abs(b) * Math.Sqrt(1 + r * r);
            }

            return r;
        }
        
        public static Double ChebEval(Int32 n, Double[] coefficients, Double x)
        {
            // If |x|  < 0.6 use the standard Clenshaw method
            if (Math.Abs(x) < 0.6)
            {
                var u0 = 0.0;
                var u1 = 0.0;
                var u2 = 0.0;
                var xx = x + x;

                for (var i = n; i >= 0; i--)
                {
                    u2 = u1;
                    u1 = u0;
                    u0 = xx * u1 + coefficients[i] - u2;
                }

                return (u0 - u2) / 2.0;
            }

            // If ABS ( T )  > =  0.6 use the Reinsch modification
            // T > =  0.6 code
            if (x > 0.0)
            {
                var u1 = 0.0;
                var d1 = 0.0;
                var d2 = 0.0;
                var xx = (x - 0.5) - 0.5;
                xx = xx + xx;

                for (var i = n; i >= 0; i--)
                {
                    d2 = d1;
                    var u2 = u1;
                    d1 = xx * u2 + coefficients[i] + d2;
                    u1 = d1 + u2;
                }

                return (d1 + d2) / 2.0;
            }
            else
            {
                // T < =  -0.6 code
                var u1 = 0.0;
                var d1 = 0.0;
                var d2 = 0.0;
                var xx = (x + 0.5) + 0.5;
                xx = xx + xx;

                for (var i = n; i >= 0; i--)
                {
                    d2 = d1;
                    var u2 = u1;
                    d1 = xx * u2 + coefficients[i] - d2;
                    u1 = d1 - u2;
                }

                return (d1 - d2) / 2.0;
            }
        }

        public static Double[,] One(Int32 dimension)
        {
            var matrix = new Double[dimension, dimension];

            for (var i = 0; i < dimension; i++)
            {
                matrix[i, i] = 1.0;
            }

            return matrix;
        }

        public static Double[,] SubMatrix(Double[,] matrix, Int32[] rows, Int32 columnStart, Int32 columnEnd)
        {
            var result = new Double[rows.Length, columnEnd - columnStart];

            for (var j = 0; j < rows.Length; j++)
            {
                for (var i = columnStart; i < columnEnd; i++)
                {
                    result[j, i - columnStart] = matrix[rows[j], i];
                }
            }

            return result;
        }

        public static Double Det2(Double[,] matrix)
        {
            return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 2];
        }

        public static Double Det3(Double[,] matrix)
        {
            return matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) +
                   matrix[0, 1] * (matrix[1, 2] * matrix[2, 0] - matrix[1, 0] * matrix[2, 2]) +
                   matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
        }

        public static Double Det4(Double[,] matrix)
        {
            //I guess that's right
            return matrix[0, 0] * (matrix[1, 1] *
                        (matrix[2, 2] * matrix[3, 3] - matrix[2, 3] * matrix[3, 2]) + matrix[1, 2] *
                            (matrix[2, 3] * matrix[3, 1] - matrix[2, 1] * matrix[3, 3]) + matrix[1, 3] *
                                (matrix[2, 1] * matrix[3, 2] - matrix[2, 2] * matrix[3, 1])) -
                    matrix[0, 1] * (matrix[1, 0] *
                        (matrix[2, 2] * matrix[3, 3] - matrix[2, 3] * matrix[3, 2]) + matrix[1, 2] *
                            (matrix[2, 3] * matrix[3, 0] - matrix[2, 0] * matrix[3, 3]) + matrix[1, 3] *
                                (matrix[2, 0] * matrix[3, 2] - matrix[2, 2] * matrix[3, 0])) +
                    matrix[0, 2] * (matrix[1, 0] *
                        (matrix[2, 1] * matrix[3, 3] - matrix[2, 3] * matrix[3, 1]) + matrix[1, 1] *
                            (matrix[2, 3] * matrix[3, 0] - matrix[2, 0] * matrix[3, 3]) + matrix[1, 3] *
                                (matrix[2, 0] * matrix[3, 1] - matrix[2, 1] * matrix[3, 0])) -
                    matrix[0, 3] * (matrix[1, 0] *
                        (matrix[2, 1] * matrix[3, 2] - matrix[2, 2] * matrix[3, 1]) + matrix[1, 1] *
                            (matrix[2, 2] * matrix[3, 0] - matrix[2, 0] * matrix[3, 2]) + matrix[1, 2] *
                                (matrix[2, 0] * matrix[3, 1] - matrix[2, 1] * matrix[3, 0]));
        }

        public static Double[,] GetColumnVector(Double[,] matrix, Int32 column)
        {
            var rows = matrix.GetLength(0);
            var result = new Double[rows, 1];

            for (var row = 0; row < rows; row++)
            {
                result[row, 0] = matrix[row, column];
            }

            return result;
        }

        public static void SetColumnVector(Double[,] matrix, Int32 column, Double[,] value)
        {
            var rows = matrix.GetLength(0);

            for (var row = 0; row < rows; row++)
            {
                matrix[row, column] = value[row, 0];
            }
        }

        public static Double[,] SubMatrix(Double[,] matrix, Int32 rowStart, Int32 rowEnd, Int32 columnStart, Int32 columnEnd)
        {
            var rows = rowEnd - rowStart;
            var cols = columnEnd - columnStart;
            var result = new Double[rows, cols];

            for (var j = rowStart; j < rowEnd; j++)
            {
                for (var i = columnStart; i < columnEnd; i++)
                {
                    result[j - rowStart, i - columnStart] = matrix[j, i];
                }
            }

            return result;
        }

        public static Double Norm(Double[,] matrix)
        {
            var sum = Reduce(matrix, matrix);
            return Math.Sqrt(sum);
        }

        public static Double[,] Transpose(Double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
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

        public static Double[,] Add(Double[,] a, Double[,] b)
        {
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
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

        public static Double[,] Subtract(Double[,] a, Double[,] b)
        {
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
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

        public static Double[,] Multiply(Double[,] a, Double b)
        {
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
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

        public static Double[,] Multiply(Double[,] a, Double[,] b)
        {
            var rows = a.GetLength(0);
            var cols = b.GetLength(1);
            var length = a.GetLength(1);

            if (length == b.GetLength(0))
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

        public static Double[,] AddScaled(Double[,] a, Double scale, Double[,] b)
        {
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
            var result = new Double[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = a[i, j] + scale * b[i, j];
                }
            }

            return result;
        }

        public static Double[,] SubtractScaled(Double[,] a, Double scale, Double[,] b)
        {
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
            var result = new Double[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = a[i, j] - scale * b[i, j];
                }
            }

            return result;
        }

        public static Double Reduce(Double[,] a, Double[,] b)
        {
            var rows = a.GetLength(0);
            var columns = a.GetLength(1);
            var sum = 0.0;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    sum += a[i, j] * b[i, j];
                }
            }

            return sum;
        }

        public static Boolean IsSymmetric(Double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);

            if (rows == cols)
            {
                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < i; j++)
                    {
                        if (matrix[i, j] != matrix[j, i])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public static Double[,] ToMatrix(Double[] p)
        {
            var l = p.Length;
            var matrix = new Double[1, l];

            for (var i = 0; i < l; i++)
            {
                matrix[0, i] = p[i];
            }

            return matrix;
        }
    }
}
