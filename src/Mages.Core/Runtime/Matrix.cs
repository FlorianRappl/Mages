namespace Mages.Core.Runtime;

using Mages.Core.Runtime.Converters;
using System;
using System.Collections.Generic;
using System.Numerics;

static class Matrix
{
    public static Int32 GetRows<T>(this T[,] matrix) => matrix.GetLength(0);

    public static Int32 GetColumns<T>(this T[,] matrix) => matrix.GetLength(1);

    public static Int32 GetCount<T>(this T[,] matrix) => matrix.GetLength(0) * matrix.GetLength(1);

    public static Object GetKeys<T>(this T[,] matrix)
    {
        var result = new Dictionary<String, Object>();
        var length = matrix.Length;

        for (var i = 0; i < length; i++)
        {
            result[i.ToString()] = (Double)i;
        }

        return result;
    }

    public static Boolean TryGetIndices<T>(this T[,] matrix, Object[] arguments, out Int32 row, out Int32 col)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        var n = 0;
        row = -1;
        col = -1;

        if (arguments.Length == 1 && arguments[0].TryGetIndex(out n))
        {
            col = n % cols;
            row = n / cols;
        }
        else if (arguments.Length == 2 && arguments[0].TryGetIndex(out row) && arguments[1].TryGetIndex(out col))
        {
        }

        return row >= 0 && col >= 0 && row < rows && col < cols;
    }
    
    public static Object Getter<T>(this T[,] matrix, Object[] arguments)
    {
        if (matrix.TryGetIndices(arguments, out var i, out var j))
        {
            return matrix[i, j];
        }

        return null;
    }

    public static void Setter(this Double[,] matrix, Object[] arguments, Object value)
    {
        if (matrix.TryGetIndices(arguments, out var i, out var j))
        {
            matrix[i, j] = value.ToNumber();
        }
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
                var n = matrix[i, j];
                sum += n * n;
            }
        }

        return Math.Sqrt(sum);
    }

    public static Double Abs(Complex[,] matrix)
    {
        var sum = 0.0;
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var c = matrix[i, j];
                sum += c.Real * c.Real + c.Imaginary * c.Imaginary;
            }
        }

        return Math.Sqrt(sum);
    }

    public static Boolean Fits<T>(this T[,] a, T[,] b)
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

    public static Complex[,] Add(this Complex[,] a, Complex[,] b)
    {
        if (a.Fits(b))
        {
            var rows = a.GetRows();
            var cols = a.GetColumns();
            var result = new Complex[rows, cols];

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

    public static Complex[,] Subtract(this Complex[,] a, Complex[,] b)
    {
        if (a.Fits(b))
        {
            var rows = a.GetRows();
            var cols = a.GetColumns();
            var result = new Complex[rows, cols];

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

    public static Complex[,] Multiply(this Complex[,] a, Complex[,] b)
    {
        var rows = a.GetRows();
        var cols = b.GetColumns();
        var length = a.GetColumns();

        if (length == b.GetRows())
        {
            var result = new Complex[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var value = Complex.Zero;

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

    public static Complex[,] Multiply(this Complex[,] a, Complex b)
    {
        var rows = a.GetRows();
        var cols = a.GetColumns();
        var result = new Complex[rows, cols];

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

    public static Complex[,] Divide(this Complex[,] a, Complex b)
    {
        return a.Multiply(1.0 / b);
    }

    public static T[,] Transpose<T>(this T[,] matrix)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        var result = new T[cols, rows];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    public static Boolean IsSquare<T>(this T[,] matrix)
    {
        return matrix.GetColumns() == matrix.GetRows();
    }

    public static T[,] Fill<T>(this T[,] matrix, T value)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        var result = new T[rows, cols];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                result[i, j] = value;
            }
        }

        return result;
    }

    public static T[,] Identity<T>(this T[,] matrix, T element)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        var result = new T[rows, cols];
        var length = Math.Min(rows, cols);

        for (var i = 0; i < length; i++)
        {
            result[i, i] = element;
        }

        return result;
    }

    public static Double[,] Pow(this Double[,] a, Double[,] b)
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
                    result[i, j] = Math.Pow(a[i, j], b[i, j]);
                }
            }

            return result;
        }

        return null;
    }

    public static Complex[,] Pow(this Complex[,] a, Complex[,] b)
    {
        if (a.Fits(b))
        {
            var rows = a.GetRows();
            var cols = a.GetColumns();
            var result = new Complex[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = Complex.Pow(a[i, j], b[i, j]);
                }
            }

            return result;
        }

        return null;
    }

    public static Double[,] Pow(this Double[,] matrix, Double value)
    {
        if (value.IsInteger() && matrix.IsSquare())
        {
            var n = (Int32)value;
            var result = matrix.Identity(1.0);

            while (n-- > 0)
            {
                result = result.Multiply(matrix);
            }

            return result;
        }

        return null;
    }

    public static Complex[,] Pow(this Complex[,] matrix, Complex value)
    {
        if (value.IsInteger() && matrix.IsSquare())
        {
            var n = (Int32)value.Real;
            var result = matrix.Identity(Complex.One);

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

    public static Complex[,] Pow(this Complex value, Complex[,] matrix)
    {
        var result = matrix.Fill(value);
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                result[i, j] = Complex.Pow(result[i, j], matrix[i, j]);
            }
        }

        return result;
    }

    public static T GetValue<T>(this T[,] matrix, Int32 row, Int32 col)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();

        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            return matrix[row, col];
        }

        return default;
    }

    public static void SetValue<T>(this T[,] matrix, Int32 row, Int32 col, T value)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();

        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            matrix[row, col] = value;
        }
    }

    public static Int32 CountAll<T>(this T[,] matrix, Func<T, Boolean> apply)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        var result = 0;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (apply(matrix[i, j]))
                {
                    result++;
                }
            }
        }

        return result;
    }

    public static T2[,] ForEach<T1, T2>(this T1[,] matrix, Func<T1, T2> apply)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        var result = new T2[rows, cols];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                result[i, j] = apply(matrix[i, j]);
            }
        }

        return result;
    }

    public static Object Reduce<T>(this T[,] matrix, Func<T, T, T> reducer)
    {
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();

        if (rows == 1 && cols == 1)
        {
            return matrix[0, 0];
        }
        else if (rows == 1)
        {
            var element = matrix[0, 0];

            for (var i = 1; i < cols; i++)
            {
                element = reducer.Invoke(element, matrix[0, i]);
            }

            return element;
        }
        else if (cols == 1)
        {
            var element = matrix[0, 0];

            for (var i = 1; i < rows; i++)
            {
                element = reducer.Invoke(element, matrix[i, 0]);
            }

            return element;
        }
        else
        {
            var result = new T[rows, 1];

            for (var i = 0; i < rows; i++)
            {
                var element = matrix[i, 0];

                for (var j = 1; j < cols; j++)
                {
                    element = reducer.Invoke(element, matrix[i, j]);
                }

                result[i, 0] = element;
            }

            return result;
        }
    }

    public static Double[,] And(this Double[,] a, Double b) => a.And(a.Fill(b));

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

    public static Double[,] IsGreaterThan(this Double[,] a, Double b) => a.IsGreaterThan(a.Fill(b));

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

    public static Double[,] IsGreaterThan(this Complex[,] a, Complex b) => a.IsGreaterThan(a.Fill(b));

    public static Double[,] IsGreaterThan(this Complex[,] a, Complex[,] b)
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
                    result[i, j] = Mathx.IsGreaterThan(a[i, j], b[i, j]).ToNumber();
                }
            }

            return result;
        }

        return null;
    }

    public static Double[,] IsGreaterOrEqual(this Double[,] a, Double b) => a.IsGreaterOrEqual(a.Fill(b));

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

    public static Double[,] IsGreaterOrEqual(this Complex[,] a, Complex b) => a.IsGreaterOrEqual(a.Fill(b));

    public static Double[,] IsGreaterOrEqual(this Complex[,] a, Complex[,] b)
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
                    result[i, j] = (!Mathx.IsGreaterThan(b[i, j], a[i, j])).ToNumber();
                }
            }

            return result;
        }

        return null;
    }

    public static Double[,] AreNotEqual(this Double[,] a, Double b) => a.AreNotEqual(a.Fill(b));

    public static Double[,] AreNotEqual(this Complex[,] a, Complex b) => a.AreNotEqual(a.Fill(b));

    public static Object Map(this Double[,] matrix, Function f)
    {
        var result = new Dictionary<String, Object>();
        var args = new Object[4];
        var rows = matrix.GetRows();
        var columns = matrix.GetColumns();

        for (int i = 0, k = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++, k++)
            {
                args[0] = matrix[i, j];
                args[1] = (Double)k;
                args[2] = (Double)i;
                args[3] = (Double)j;
                result[k.ToString()] = f(args);
            }
        }

        return result;
    }

    public static Object Where(this Double[,] matrix, Function f)
    {
        var result = new List<Double>();
        var args = new Object[4];
        var rows = matrix.GetRows();
        var columns = matrix.GetColumns();

        for (int i = 0, k = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++, k++)
            {
                var value = matrix[i, j];
                args[0] = value;
                args[1] = (Double)k;
                args[2] = (Double)i;
                args[3] = (Double)j;

                if (f(args).ToBoolean())
                {
                    result.Add(value);
                }
            }
        }

        return result.ToMatrix();
    }

    public static Object Reduce(this Double[,] matrix, Function f, Object start)
    {
        var args = new Object[5];
        var result = start;
        var rows = matrix.GetRows();
        var columns = matrix.GetColumns();

        for (int i = 0, k = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++, k++)
            {
                args[0] = result;
                args[1] = matrix[i, j];
                args[2] = (Double)k;
                args[3] = (Double)i;
                args[4] = (Double)j;
                result = f(args);
            }
        }

        return result;
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

    public static Double[,] AreNotEqual(this Complex[,] a, Complex[,] b)
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

    public static Double[,] IsLessThan(this Double[,] a, Double b) => a.IsLessThan(a.Fill(b));

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

    public static Double[,] IsLessThan(this Complex[,] a, Complex b) => a.IsLessThan(a.Fill(b));

    public static Double[,] IsLessThan(this Complex[,] a, Complex[,] b)
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
                    result[i, j] = Mathx.IsGreaterThan(b[i, j], a[i, j]).ToNumber();
                }
            }

            return result;
        }

        return null;
    }

    public static Double[,] IsLessOrEqual(this Double[,] a, Double b) => a.IsLessOrEqual(a.Fill(b));

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

    public static Double[,] IsLessOrEqual(this Complex[,] a, Complex b) => a.IsLessOrEqual(a.Fill(b));

    public static Double[,] IsLessOrEqual(this Complex[,] a, Complex[,] b)
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
                    result[i, j] = (!Mathx.IsGreaterThan(a[i, j], b[i, j])).ToNumber();
                }
            }

            return result;
        }

        return null;
    }

    public static Double[,] AreEqual(this Double[,] a, Double b) => a.AreEqual(a.Fill(b));

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

    public static Double[,] AreEqual(this Complex[,] a, Complex b) => a.AreEqual(a.Fill(b));

    public static Double[,] AreEqual(this Complex[,] a, Complex[,] b)
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

    public static Double[,] Or(this Double[,] a, Double b) => a.Or(a.Fill(b));

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
