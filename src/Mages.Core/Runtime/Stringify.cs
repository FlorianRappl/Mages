namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public static class Stringify
    {
        public static String This(Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String This(String value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

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

        public static String This(IDictionary<String, Object> value)
        {
            return "[Object]";
        }

        public static String This(Function value)
        {
            return "[Function]";
        }

        public static String Undefined()
        {
            return System.String.Empty;
        }

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
