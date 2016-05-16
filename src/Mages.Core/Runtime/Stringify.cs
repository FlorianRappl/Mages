namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    static class Stringify
    {
        public static String Number(this Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String String(this String value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String Matrix(this Double[,] value)
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

        public static String Object(this IDictionary<String, Object> value)
        {
            return "[Object]";
        }

        public static String Function(this Function value)
        {
            return "[Function]";
        }

        public static String Undefined()
        {
            return System.String.Empty;
        }
    }
}
