namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    static class ConverterExtensions
    {
        public static Boolean ToBoolean(this Double value)
        {
            return value != 0.0;
        }

        public static Boolean ToBoolean(this String value)
        {
            return value.Length > 0;
        }

        public static Boolean ToBoolean(this Double[,] matrix)
        {
            foreach (var value in matrix)
            {
                if (value != 0.0)
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean ToBoolean(this IDictionary<String, Object> obj)
        {
            return obj.Count > 0;
        }

        public static Double ToNumber(this Boolean value)
        {
            return value ? 1.0 : 0.0;
        }

        public static Object ToNumber(this String value)
        {
            var result = default(Double);

            if (!Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return null;
            }

            return result;
        }

        public static Object ToNumber(this Double[,] matrix)
        {
            if (matrix.GetLength(0) == 1 && matrix.GetLength(1) == 1)
            {
                return matrix[0, 0];
            }

            return null;
        }

        public static Double[,] ToMatrix(this Double value)
        {
            return new Double[1, 1] { { value } };
        }

        public static Double[,] ToMatrix(this Boolean value)
        {
            return new Double[1, 1] { { value.ToNumber() } };
        }

        public static Double[,] ToMatrix(this IEnumerable<Double> value)
        {
            var source = value.ToList();
            var length = source.Count;
            var matrix = new Double[1, length];

            for (var i = 0; i < length; i++)
            {
                matrix[0, i] = source[i];
            }

            return matrix;
        }

        public static Double[] ToVector(this Double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var vec = new Double[rows * cols];
            var k = 0;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    vec[k++] = matrix[i, j];
                }
            }

            return vec;
        }

        public static List<Double> ToList(this Double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var list = new List<Double>(rows * cols);

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    list.Add(matrix[i, j]);
                }
            }

            return list;
        }
    }
}
