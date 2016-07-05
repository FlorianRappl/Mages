namespace Mages.Core.Runtime
{
    using System;

    static class Mathx
    {
        public static Double Sign(Double value)
        {
            return (Double)Math.Sign(value);
        }

        public static Double Factorial(Double value)
        {
            var result = (Double)Math.Sign(value);
            var n = (Int32)Math.Floor(result * value);

            while (n > 0)
            {
                result *= n--;
            }

            return result;
        }

        public static Double Asinh(Double v)
        {
            return Math.Log(v + Math.Sqrt(v * v + 1.0));
        }

        public static Double Acosh(Double v)
        {
            return Math.Log(v + Math.Sqrt(v * v - 1.0));
        }

        public static Double Atanh(Double v)
        {
            return 0.5 * Math.Log((1.0 + v) / (1.0 - v));
        }

        public static Double Cot(Double v)
        {
            return Math.Cos(v) / Math.Sin(v);
        }

        public static Double Acot(Double v)
        {
            return Math.Atan(1.0 / v);
        }

        public static Double Coth(Double v)
        {
            var a = Math.Exp(+v);
            var b = Math.Exp(-v);
            return (a + b) / (a - b);
        }

        public static Double Acoth(Double v)
        {
            return 0.5 * Math.Log((1.0 + v) / (v - 1.0));
        }

        public static Double Sec(Double v)
        {
            return 1.0 / Math.Cos(v);
        }

        public static Double Asec(Double v)
        {
            return Math.Acos(1.0 / v);
        }

        public static Double Sech(Double v)
        {
            return 2.0 / (Math.Exp(v) + Math.Exp(-v));
        }

        public static Double Asech(Double v)
        {
            var vi = 1.0 / v;
            return Math.Log(vi + Math.Sqrt(vi + 1.0) * Math.Sqrt(vi - 1.0));
        }

        public static Double Csc(Double v)
        {
            return 1.0 / Math.Sin(v);
        }

        public static Double Acsc(Double v)
        {
            return Math.Asin(1.0 / v);
        }

        public static Double Csch(Double v)
        {
            return 2.0 / (Math.Exp(v) - Math.Exp(-v));
        }

        public static Double Acsch(Double v)
        {
            return Math.Log(1.0 / v + Math.Sqrt(1.0 / (v * v) + 1.0));
        }
    }
}
