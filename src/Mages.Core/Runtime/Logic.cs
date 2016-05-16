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
    }
}
