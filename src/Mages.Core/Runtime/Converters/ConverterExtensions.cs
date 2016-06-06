namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    static class ConverterExtensions
    {
        public static String ToType(this Object value)
        {
            if (value is Double) return "Number";
            if (value is String) return "String";
            if (value is Boolean) return "Boolean";
            if (value is Double[,]) return "Matrix";
            if (value is Function) return "Function";
            if (value is IDictionary<String, Object>) return "Object";
            return "Undefined";
        }

        public static Object To(this Object value, String type)
        {
            switch (type)
            {
                case "Number": return value.ToNumber();
                case "String": return Stringify.This(value);
                case "Boolean": return value.ToBoolean();
                case "Matrix": return value.ToNumber().ToMatrix();
                case "Function": return value as Function;
                case "Object": return value as IDictionary<String, Object>;
                case "Undefined": return null;
            }

            return null;
        }

        public static Boolean ToBoolean(this Object value)
        {
            if (value is Boolean)
            {
                return (Boolean)value;
            }
            else if (value != null)
            {
                var nval = value as Double?;
                var sval = value as String;
                var mval = value as Double[,];
                var oval = value as IDictionary<String, Object>;

                if (nval.HasValue)
                {
                    return nval.Value.ToBoolean();
                }
                else if (sval != null)
                {
                    return sval.ToBoolean();
                }
                else if (mval != null)
                {
                    return mval.ToBoolean();
                }
                else if (oval != null)
                {
                    return oval.ToBoolean();
                }

                return true;
            }

            return false;
        }

        public static Boolean ToBoolean(this Double value)
        {
            return value != 0.0;;
        }

        public static Boolean ToBoolean(this String value)
        {
            return value.Length > 0;
        }

        public static Boolean ToBoolean(this Double[,] matrix)
        {
            return matrix.AnyTrue();
        }

        public static Boolean ToBoolean(this IDictionary<String, Object> obj)
        {
            return obj.Count > 0;
        }

        public static Double ToNumber(this Object value)
        {
            if (value is Double)
            {
                return (Double)value;
            }
            else if (value != null)
            {
                var bval = value as Boolean?;
                var sval = value as String;
                var mval = value as Double[,];

                if (bval.HasValue)
                {
                    return bval.Value.ToNumber();
                }
                else if (sval != null)
                {
                    return sval.ToNumber();
                }
                else if (mval != null)
                {
                    return mval.ToNumber();
                }
            }

            return Double.NaN;
        }

        public static Double ToNumber(this Boolean value)
        {
            return value ? 1.0 : 0.0;
        }

        public static Double ToNumber(this String value)
        {
            var result = default(Double);

            if (!Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return Double.NaN;
            }

            return result;
        }

        public static Double ToNumber(this Double[,] matrix)
        {
            if (matrix.GetRows() == 1 && matrix.GetColumns() == 1)
            {
                return matrix[0, 0];
            }

            return Double.NaN;
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
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
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

        public static Boolean TryGetIndex(this Object obj, out Int32 value)
        {
            if (obj is Double && ((Double)obj).IsInteger())
            {
                value = (Int32)(Double)obj;
                return true;
            }
            else
            {
                value = -1;
                return false;
            }   
        }
    }
}
