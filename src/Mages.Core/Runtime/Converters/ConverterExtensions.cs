namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// A set of useful extension methods for type conversions.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Returns the type of the given value.
        /// </summary>
        /// <param name="value">The value to get the type of.</param>
        /// <returns>The MAGES type string.</returns>
        public static String ToType(this Object value) => value switch
        {
            Double _ => "Number",
            Complex _ => "Complex",
            String _ => "String",
            Boolean _ => "Boolean",
            Double[,] or Complex[,] _ =>"Matrix",
            Function _ => "Function",
            IDictionary<String, Object> _ => "Object",
            _ => "Undefined",
        };

        /// <summary>
        /// Converts the given value to the specified type.
        /// </summary>
        /// <param name="value">The type to convert.</param>
        /// <param name="type">The destination type.</param>
        /// <returns>The converted value.</returns>
        public static Object To(this Object value, String type) => type switch
        {
            "Number" => value.ToNumber(),
            "String" => Stringify.This(value),
            "Boolean" => value.ToBoolean(),
            "Matrix" => value.ToNumber().ToMatrix(),
            "Function" => value as Function,
            "Object" => value as IDictionary<String, Object>,
            "Undefined" => null,
            _ => null,
        };

        /// <summary>
        /// Returns the boolean representation of the given value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The boolean representation of the value.</returns>
        public static Boolean ToBoolean(this Object value)
        {
            if (value is Boolean bval)
            {
                return bval;
            }
            else if (value != null)
            {
                var nval = value as Double?;

                if (nval.HasValue)
                {
                    return nval.Value.ToBoolean();
                }
                else if (value is String sval)
                {
                    return sval.ToBoolean();
                }
                else if (value is Double[,] mval)
                {
                    return mval.ToBoolean();
                }
                else if (value is IDictionary<String, Object> oval)
                {
                    return oval.ToBoolean();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the boolean representation of the given numeric value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The boolean representation of the value.</returns>
        public static Boolean ToBoolean(this Double value) => value != 0.0;

        /// <summary>
        /// Returns the boolean representation of the given string value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The boolean representation of the value.</returns>
        public static Boolean ToBoolean(this String value) => value.Length > 0;

        /// <summary>
        /// Returns the boolean representation of the given matrix value.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>The boolean representation of the value.</returns>
        public static Boolean ToBoolean(this Double[,] matrix) => matrix.AnyTrue();

        /// <summary>
        /// Returns the boolean representation of the given object value.
        /// </summary>
        /// <param name="obj">The obj to convert.</param>
        /// <returns>The boolean representation of the value.</returns>
        public static Boolean ToBoolean(this IDictionary<String, Object> obj) => obj.Count > 0;

        /// <summary>
        /// Returns the object representation of the given value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The object representation of the value.</returns>
        public static IDictionary<String, Object> ToObject(this Object value)
        {
            if (value is IDictionary<String, Object> dict)
            {
                return dict;
            }
            else if (value is Double[,] matrix)
            {
                var result = new Dictionary<String, Object>();
                var rows = matrix.GetRows();
                var columns = matrix.GetColumns();

                for (int i = 0, k = 0; i < rows; i++)
                {
                    for (var j = 0; j < columns; j++, k++)
                    {
                        result[k.ToString()] = matrix[i, j];
                    }
                }

                return result;
            }
            else
            {
                var result = new Dictionary<String, Object>();

                if (value != null)
                {
                    result["0"] = value;
                }

                return result;
            }
        }

        /// <summary>
        /// Returns the number representation of the given value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The number representation of the value.</returns>
        public static Double ToNumber(this Object value)
        {
            if (value is Double dval)
            {
                return dval;
            }
            else if (value != null)
            {
                var bval = value as Boolean?;

                if (bval.HasValue)
                {
                    return bval.Value.ToNumber();
                }
                else if (value is String sval)
                {
                    return sval.ToNumber();
                }
                else if (value is Double[,] mval)
                {
                    return mval.ToNumber();
                }
            }

            return Double.NaN;
        }

        /// <summary>
        /// Returns the complex representation of the given value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The complex representation of the value.</returns>
        public static Complex ToComplex(this Object value)
        {
            if (value is Complex c)
            {
                return c;
            }

            return value.ToNumber();
        }

        /// <summary>
        /// Returns the number representation of the given boolean value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The number representation of the value.</returns>
        public static Double ToNumber(this Boolean value) => value ? 1.0 : 0.0;

        /// <summary>
        /// Returns the complex representation of the given boolean value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The complex representation of the value.</returns>
        public static Complex ToComplex(this Boolean value) => value ? Complex.One : Complex.Zero;

        /// <summary>
        /// Returns the number representation of the given string value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The number representation of the value.</returns>
        public static Double ToNumber(this String value)
        {
            if (!Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return Double.NaN;
            }

            return result;
        }

        /// <summary>
        /// Returns the number representation of the given matrix value.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>The number representation of the value.</returns>
        public static Double ToNumber(this Double[,] matrix)
        {
            if (matrix.GetRows() == 1 && matrix.GetColumns() == 1)
            {
                return matrix[0, 0];
            }

            return Double.NaN;
        }

        /// <summary>
        /// Returns the number representation of the given matrix value.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>The number representation of the value.</returns>
        public static Complex ToNumber(this Complex[,] matrix)
        {
            if (matrix.GetRows() == 1 && matrix.GetColumns() == 1)
            {
                return matrix[0, 0];
            }

            return Double.NaN;
        }

        /// <summary>
        /// Returns the matrix representation of the given number value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The matrix representation of the value.</returns>
        public static Double[,] ToMatrix(this Double value) => new Double[1, 1] { { value } };

        /// <summary>
        /// Returns the matrix representation of the given number value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The matrix representation of the value.</returns>
        public static Complex[,] ToMatrix(this Complex value) => new Complex[1, 1] { { value } };

        /// <summary>
        /// Returns the matrix representation of the given boolean value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The matrix representation of the value.</returns>
        public static Double[,] ToMatrix(this Boolean value) => new Double[1, 1] { { value.ToNumber() } };

        /// <summary>
        /// Returns the matrix representation of the given numeric values.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The matrix representation of the value.</returns>
        public static T[,] ToMatrix<T>(this IEnumerable<T> value)
        {
            var source = value.ToList();
            return source.ToMatrix();
        }

        /// <summary>
        /// Returns the matrix representation of the given numeric values.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The matrix representation of the value.</returns>
        public static T[,] ToMatrix<T>(this List<T> value)
        {
            var length = value.Count;
            var matrix = new T[1, length];

            for (var i = 0; i < length; i++)
            {
                matrix[0, i] = value[i];
            }

            return matrix;
        }

        /// <summary>
        /// Returns the vector representation of the given matrix value.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>The matrix representation of the value.</returns>
        public static T[] ToVector<T>(this T[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var vec = new T[rows * cols];
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

        /// <summary>
        /// Returns the list representation of the given matrix value.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>The list representation of the value.</returns>
        public static List<T> ToList<T>(this T[,] matrix)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
            var list = new List<T>(rows * cols);

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    list.Add(matrix[i, j]);
                }
            }

            return list;
        }

        /// <summary>
        /// Tries to get the index supplied from the given object.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <param name="value">The retrieved index.</param>
        /// <returns>True if the index could be retrieved, otherwise false.</returns>
        public static Boolean TryGetIndex(this Object obj, out Int32 value)
        {
            if (obj is Double val && val.IsInteger())
            {
                value = (Int32)val;
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
