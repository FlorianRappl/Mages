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
        /// Contains the stringify function.
        /// </summary>
        public static readonly Function Default = new Function(args => This(args.Length > 0 ? args[0] : null));

        /// <summary>
        /// Contains the JSON function.
        /// </summary>
        public static readonly Function Json = new Function(args => AsJson(args.Length > 0 ? args[0] : null));

        /// <summary>
        /// Converts the number to a string.
        /// </summary>
        public static String This(Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the boolean to a string.
        /// </summary>
        public static String This(Boolean value)
        {
            return value ? Keywords.True : Keywords.False;
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
                return This((IDictionary<String, Object>)value);
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
            else if (value is Boolean)
            {
                return This((Boolean)value);
            }

            return "(unknown)";
        }

        /// <summary>
        /// Converts the given MAGES object to a JSON string.
        /// </summary>
        /// <param name="value">The object to represent.</param>
        /// <returns>The string representation.</returns>
        public static String AsJson(Object value)
        {
            if (value == null)
            {
                return "null";
            }
            else if (value is Function)
            {
                return AsJson("[Function]");
            }
            else if (value is IDictionary<String, Object>)
            {
                return AsJson((IDictionary<String, Object>)value);
            }
            else if (value is Double[,])
            {
                return AsJson((Double[,])value);
            }
            else if (value is String)
            {
                return AsJson((String)value);
            }
            else if (value is Double)
            {
                return This((Double)value);
            }
            else if (value is Boolean)
            {
                return This((Boolean)value);
            }

            return "undefined";
        }

        public static String AsJson(String str)
        {
            var escaped = str.Replace("\"", "\\\"");
            return String.Concat("\"", escaped, "\"");
        }

        public static String AsJson(IDictionary<String, Object> obj)
        {
            var sb = StringBuilderPool.Pull();
            sb.AppendLine("{");

            foreach (var item in obj)
            {
                var key = AsJson(item.Key);
                var value = AsJson(item.Value);

                if (value.Contains(Environment.NewLine))
                {
                    var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    value = String.Join(Environment.NewLine + "  ", lines);
                }

                sb.Append("  ").Append(key).Append(": ").AppendLine(value);
            }

            sb.Append('}');
            return sb.Stringify();
        }

        public static String AsJson(Double[,] matrix)
        {
            var sb = StringBuilderPool.Pull();
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();
            sb.Append('[');

            for (var i = 0; i < rows; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append('[');

                for (var j = 0; j < cols; j++)
                {
                    if (j > 0) sb.Append(", ");
                    sb.Append(This(matrix[i, j]));
                }

                sb.Append(']');
            }

            sb.Append(']');
            return sb.Stringify();
        }
    }
}
