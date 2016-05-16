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
    }
}
