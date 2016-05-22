namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;

    static class Helpers
    {
        public static void SetValue(this Double[,] matrix, Int32 i, Int32 j, Double value)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);

            if (i >= 0 && i < rows && j >= 0 && j < cols)
            {
                matrix[i, j] = value;
            }
        }

        public static Double[,] Transpose(this Double[,] matrix)
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

        public static Double GetValue(this Double[,] matrix, Int32 i, Int32 j)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);

            if (i >= 0 && i < rows && j >= 0 && j < cols)
            {
                return matrix[i, j];
            }

            return 0.0;
        }

        public static void SetProperty(this IDictionary<String, Object> obj, String name, Object value)
        {
            obj[name] = value;
        }

        public static Object GetProperty(this IDictionary<String, Object> obj, String name)
        {
            var value = default(Object);
            obj.TryGetValue(name, out value);
            return value;
        }

        public static Double Factorial(this Double value)
        {
            var result = Math.Sign(value);
            var n = (Int32)Math.Floor(result * value);

            while (n > 0)
            {
                result *= n--;
            }

            return result;
        }

        public static Function Wrap<TArg, TRes>(Func<TArg, TRes> func)
        {
            return Wrap((Delegate)func);
        }

        public static Function Wrap(Delegate func)
        {
            return new Function(func.DynamicInvoke);
        }
    }
}
