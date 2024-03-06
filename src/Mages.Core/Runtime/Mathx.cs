namespace Mages.Core.Runtime;

using System;
using System.Numerics;

static class Mathx
{
    public static readonly Double[] LanczosD = new[]
    {
         2.48574089138753565546e-5,
         1.05142378581721974210,
        -3.45687097222016235469,
         4.51227709466894823700,
        -2.98285225323576655721,
         1.05639711577126713077,
        -1.95428773191645869583e-1,
         1.70970543404441224307e-2,
        -5.71926117404305781283e-4,
         4.63399473359905636708e-6,
        -2.71994908488607703910e-9
    };

    public static readonly Double[] BernoulliNumbers = new[]
    {
        1.0,
        1.0 / 6.0,
        -1.0 / 30.0,
        1.0 / 42.0,
        -1.0 / 30.0,
        5.0 / 66.0,
        -691.0 / 2730.0,
        7.0 / 6.0,
        -3617.0 / 510.0,
        43867.0 / 798.0,
        -174611.0 / 330.0,
        854513.0 / 138.0,
        -236364091.0 / 2730.0,
        8553103.0 / 6.0,
        -23749461029.0 / 870.0,
        8615841276005.0 / 14322.0,
        -7709321041217.0 / 510.0,
        2577687858367.0 / 6.0,
        -26315271553053477373.0 / 1919190.0,
        2929993913841559.0 / 6.0,
        -261082718496449122051.0 / 13530.0
    };

    public const Double LanczosR = 10.900511;

    public static Boolean IsGreaterThan(Complex a, Complex b) => Complex.Abs(a) > Complex.Abs(b);

    public static Boolean IsLessThan(Complex a, Complex b) => Complex.Abs(a) < Complex.Abs(b);

    public static Complex Min(Complex a, Complex b) => IsGreaterThan(a, b) ? b : a;

    public static Complex Max(Complex a, Complex b) => IsGreaterThan(a, b) ? a : b;

    public static Complex Ceiling(Complex value) => new(Math.Ceiling(value.Real), Math.Ceiling(value.Imaginary));

    public static Complex Floor(Complex value) => new(Math.Floor(value.Real), Math.Floor(value.Imaginary));

    public static Complex Round(Complex value) => new(Math.Round(value.Real), Math.Round(value.Imaginary));

    public static Double Sign(Double value) => (Double)Math.Sign(value);

    public static Object Sqrt(Double x)
    {
        if (x >= 0)
        {
            return Math.Sqrt(x);
        }

        return Complex.Sqrt(x);
    }

    public static Object Sqrt(Double[,] x)
    {
        if (x.HasAny(x => x < 0.0))
        {
            return x.ForEach(m => Complex.Sqrt(m));
        }

        return x.ForEach(Math.Sqrt);
    }

    public static Complex Sign(Complex value)
    {
        var arg = Math.Atan2(value.Imaginary, value.Real);
        return new Complex(Math.Cos(arg), Math.Sin(arg));
    }

    public static Complex Mod(Complex a, Complex b)
    {
        var x = a / b;
        var z = new Complex(Math.Floor(x.Real), Math.Floor(x.Imaginary)) * b;
        return a - z;
    }

    public static Complex Factorial(Complex value) => new(Factorial(value.Real), Factorial(value.Imaginary));

    public static Double Factorial(Double value)
    {
        if (value.IsInteger() && Math.Abs(value) < 1e5)
        {
            var result = (Double)Math.Sign(value);
            var n = (Int32)Math.Floor(result * value);

            if (n == 0)
            {
                return 1.0;
            }

            while (n > 0)
            {
                result *= n--;
            }

            return result;
        }
        else
        {
            return Gamma(value + 1.0);
        }
    }

    public static Double Gamma(Double x) => GammaHelpers.LinearGamma(x);

    public static Complex Gamma(Complex x) => GammaHelpers.LinearGamma(x);

    public static Double Asinh(Double value) => Math.Log(value + Math.Sqrt(value * value + 1.0));

    public static Complex Asinh(Complex value) => Complex.Log(value + Complex.Sqrt(value * value + 1.0));

    private static Double AcoshReal(Double value) => Math.Log(value + Math.Sqrt(value * value - 1.0));

    private static Complex AcoshCmplx(Double value) => Acosh(new Complex(value, 0));

    public static Object Acosh(Double value)
    {
        if (value >= 1.0)
        {
            return AcoshReal(value);
        }

        return AcoshCmplx(value);
    }

    public static Object Acosh(Double[,] value)
    {
        if (value.HasAny(x => x < 1.0))
        {
            return value.ForEach(AcoshCmplx);
        }

        return value.ForEach(AcoshReal);
    }

    public static Complex Acosh(Complex value) => Complex.Log(value + Complex.Sqrt(value * value - 1.0));

    private static Double AtanhReal(Double value) => 0.5 * Math.Log((1.0 + value) / (1.0 - value));

    private static Complex AtanhCmplx(Double value) => Atanh(new Complex(value, 0));

    public static Object Atanh(Double value)
    {
        if (value >= -1.0 && value <= 1.0)
        {
            return AtanhReal(value);
        }

        return AtanhCmplx(value);
    }

    public static Object Atanh(Double[,] value)
    {
        if (value.HasAny(x => x < -1.0 || x > 1.0))
        {
            return value.ForEach(AtanhCmplx);
        }

        return value.ForEach(AtanhReal);
    }

    public static Complex Atanh(Complex value) => 0.5 * Complex.Log((1.0 + value) / (1.0 - value));

    public static Double Cot(Double value) => Math.Cos(value) / Math.Sin(value);

    public static Complex Cot(Complex value) => Complex.Cos(value) / Complex.Sin(value);

    public static Double Acot(Double value) => Math.Atan(1.0 / value);

    public static Complex Acot(Complex value) => Complex.Atan(1.0 / value);

    public static Double Coth(Double value)
    {
        var a = Math.Exp(+value);
        var b = Math.Exp(-value);
        return (a + b) / (a - b);
    }

    public static Complex Coth(Complex value)
    {
        var a = Complex.Exp(value);
        var b = Complex.Exp(-value);
        return (a + b) / (a - b);
    }

    private static Double AcothReal(Double value) => 0.5 * Math.Log((1.0 + value) / (value - 1.0));

    private static Complex AcothCmplx(Double value) => Acoth(new Complex(value, 0.0));

    public static Object Acoth(Double value)
    {
        if (value <= -1.0 || value >= 1.0)
        {
            return AcothReal(value);
        }

        return AcothCmplx(value);
    }

    public static Object Acoth(Double[,] value)
    {
        if (value.HasAny(x => x > -1.0 && x < 1.0))
        {
            return value.ForEach(AcothCmplx);
        }

        return value.ForEach(AcothReal);
    }

    public static Complex Acoth(Complex value) => 0.5 * Complex.Log((1.0 + value) / (value - 1.0));

    public static Double Sec(Double value) => 1.0 / Math.Cos(value);

    public static Complex Sec(Complex value) => 1.0 / Complex.Cos(value);

    private static Double AsecReal(Double value) => Math.Acos(1.0 / value);

    private static Complex AsecCmplx(Double value) => Asec(new Complex(value, 0.0));

    public static Object Asec(Double value)
    {
        if (value >= 1.0)
        {
            return AsecReal(value);
        }

        return AsecCmplx(value);
    }

    public static Object Asec(Double[,] value)
    {
        if (value.HasAny(x => x < 1.0))
        {
            return value.ForEach(AsecCmplx);
        }

        return value.ForEach(AsecReal);
    }

    private static Double AsinReal(Double value) => Math.Asin(value);

    private static Complex AsinCmplx(Double value) => Complex.Asin(new Complex(value, 0.0));

    public static Object Asin(Double value)
    {
        if (value >= -1.0 && value <= 1.0)
        {
            return AsinReal(value);
        }

        return AsinCmplx(value);
    }

    public static Object Asin(Double[,] value)
    {
        if (value.HasAny(x => x < -1.0 || x > 1.0))
        {
            return value.ForEach(AsinCmplx);
        }

        return value.ForEach(AsinReal);
    }

    private static Double AcosReal(Double value) => Math.Acos(value);

    private static Complex AcosCmplx(Double value) => Complex.Acos(new Complex(value, 0.0));

    public static Object Acos(Double value)
    {
        if (value >= -1.0 && value <= 1.0)
        {
            return AcosReal(value);
        }

        return AcosCmplx(value);
    }

    public static Object Acos(Double[,] value)
    {
        if (value.HasAny(x => x < -1.0 || x > 1.0))
        {
            return value.ForEach(AcosCmplx);
        }

        return value.ForEach(AcosReal);
    }

    public static Complex Asec(Complex value) => Complex.Acos(1.0 / value);

    public static Double Sech(Double value) => 2.0 / (Math.Exp(value) + Math.Exp(-value));

    public static Complex Sech(Complex value) => 2.0 / (Complex.Exp(value) + Complex.Exp(-value));

    public static Double Asech(Double value)
    {
        var vi = 1.0 / value;
        return Math.Log(vi + Math.Sqrt(vi + 1.0) * Math.Sqrt(vi - 1.0));
    }

    public static Complex Asech(Complex value)
    {
        var vi = 1.0 / value;
        return Complex.Log(vi + Complex.Sqrt(vi + 1.0) * Complex.Sqrt(vi - 1.0));
    }

    public static Double Csc(Double value) => 1.0 / Math.Sin(value);

    public static Complex Csc(Complex value) => 1.0 / Complex.Sin(value);

    private static Double AcscReal(Double value) => Math.Asin(1.0 / value);

    private static Complex AcscCmplx(Double value) => Acsc(new Complex(value, 0.0));

    public static Object Acsc(Double value)
    {
        if (value <= -1.0 || value >= 1.0)
        {
            return AcscReal(value);
        }

        return AcscCmplx(value);
    }

    public static Object Acsc(Double[,] value)
    {
        if (value.HasAny(x => x > -1.0 && x < 1.0))
        {
            return value.ForEach(AcscCmplx);
        }

        return value.ForEach(AcscReal);
    }

    public static Complex Acsc(Complex value) => Complex.Asin(1.0 / value);

    public static Double Csch(Double value) => 2.0 / (Math.Exp(value) - Math.Exp(-value));

    public static Complex Csch(Complex value) => 2.0 / (Complex.Exp(value) - Complex.Exp(-value));

    public static Double Acsch(Double value) => Math.Log(1.0 / value + Math.Sqrt(1.0 / (value * value) + 1.0));

    public static Complex Acsch(Complex value) => Complex.Log(1.0 / value + Complex.Sqrt(1.0 / (value * value) + 1.0));

    public static Object Log(Double value)
    {
        if (value >= 0.0)
        {
            return Math.Log(value);
        }

        return Complex.Log(value);
    }

    public static Object Log(Double[,] value)
    {
        if (value.HasAny(m => m < 0.0))
        {
            return value.ForEach(m => Complex.Log(m));
        }

        return value.ForEach(Math.Log);
    }

    public static Object Log2(Double value)
    {
        if (value >= 0.0)
        {
            return Math.Log(value, 2.0);
        }

        return Complex.Log(value, 2.0);
    }

    public static Object Log2(Double[,] value)
    {
        if (value.HasAny(m => m < 0.0))
        {
            return value.ForEach(m => Complex.Log(m, 2.0));
        }

        return value.ForEach(m => Math.Log(m, 2.0));
    }

    public static Complex Log2(Complex value) => Complex.Log(value, 2.0);

    public static Object Log10(Double value)
    {
        if (value >= 0.0)
        {
            return Math.Log10(value);
        }

        return Complex.Log10(value);
    }

    public static Object Log10(Double[,] value)
    {
        if (value.HasAny(m => m < 0.0))
        {
            return value.ForEach(m => Complex.Log10(m));
        }

        return value.ForEach(Math.Log10);
    }
}
