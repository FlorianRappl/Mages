namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;

    static class Logic
    {
        public static Boolean IsTrue(this Double value)
        {
            return !value.IsFalse();
        }

        public static Boolean IsFalse(this Double value)
        {
            return value == 0.0;
        }

        public static Boolean IsTrue(this String value)
        {
            return !value.IsFalse();
        }

        public static Boolean IsFalse(this String value)
        {
            return value.Length == 0;
        }

        public static Boolean IsTrue(this IDictionary<String, Object> value)
        {
            return !value.IsFalse();
        }

        public static Boolean IsFalse(this IDictionary<String, Object> value)
        {
            return value.Count == 0;
        }

        public static Boolean IsPrime(this Double value)
        {
            if (value.IsInteger())
            {
                return PrimeNumber.Check((Int32)value);
            }

            return false;
        }

        public static Boolean IsInteger(this Double value)
        {
            return Math.Truncate(value) == value;
        }
    }
}
