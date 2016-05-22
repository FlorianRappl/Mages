namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Helpers to stringify objects used by MAGES.
    /// </summary>
    public static class Stringify
    {
        /// <summary>
        /// Converts the number to a string.
        /// </summary>
        public static String This(Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the string for output.
        /// </summary>
        public static String This(String value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the matrix to a string.
        /// </summary>
        public static String This(Double[,] value)
        {
            var sb = StringBuilderPool.Pull();
            var rows = value.GetLength(0);
            var cols = value.GetLength(1);
            sb.Append('[');

            for (var i = 0; i < rows; i++)
            {
                if (i > 0)
                {
                    sb.Append(';');
                }

                for (var j = 0; j < cols; j++)
                {
                    if (j > 0)
                    {
                        sb.Append(',');
                    }

                    sb.Append(value[i, j].ToString(CultureInfo.InvariantCulture));
                }
            }

            sb.Append(']');
            return sb.Stringify();
        }

        /// <summary>
        /// Converts the object to a string.
        /// </summary>
        public static String This(IDictionary<String, Object> value)
        {
            return "[Object]";
        }

        /// <summary>
        /// Converts the function to a string.
        /// </summary>
        public static String This(Function value)
        {
            return "[Function]";
        }

        /// <summary>
        /// Outputs the string for an undefined (null?) value.
        /// </summary>
        /// <returns></returns>
        public static String Undefined()
        {
            return System.String.Empty;
        }

        /// <summary>
        /// Converts the undetermined value to a string.
        /// </summary>
        public static String This(Object value)
        {
            if (value == null)
            {
                return Undefined();
            }
            else if (value is Function)
            {
                return This((Function)value);
            }
            else if (value is IDictionary<String, Object>)
            {
                return This((Dictionary<String, Object>)value);
            }
            else if (value is Double[,])
            {
                return This((Double[,])value);
            }
            else if (value is String)
            {
                return This((String)value);
            }
            else if (value is Double)
            {
                return This((Double)value);
            }

            return "(unknown)";
        }
    }
}
