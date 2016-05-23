namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

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
    }
}
