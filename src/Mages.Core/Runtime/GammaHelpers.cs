namespace Mages.Core.Runtime;

using System;
using System.Numerics;

/// <summary>
/// This class contains the linear gamma function as well as complex ones
/// and logarithmic ones.
/// </summary>
public static class GammaHelpers
{
    #region Linear

    /// <summary>
    /// Computes the real (linear) gamma function.
    /// </summary>
    /// <param name="x">The argument.</param>
    /// <returns>The evaluated value.</returns>
    public static Double LinearGamma(Double x)
    {
        if (x <= 0.0)
        {
            if (x == Math.Ceiling(x))
            {
                return Double.PositiveInfinity;
            }

            return Math.PI / LinearGamma(-x) / (-x) / Math.Sin(x * Math.PI);
        }

        return Math.Exp(LogGamma(x));
    }

    /// <summary>
    /// Computes the complex (linear) gamma function.
    /// </summary>
    /// <param name="z">The complex argument.</param>
    /// <returns>The evaluated value.</returns>
		public static Complex LinearGamma(Complex z)
    {
        if (z.Real < 0.5)
        {
            return Math.PI / LinearGamma(1.0 - z) / Complex.Sin(Math.PI * z);
        }

        return Complex.Exp(LogGamma(z));
    }

    #endregion

    #region Log

    /// <summary>
    /// Computes the real (log) gamma function.
    /// </summary>
    /// <param name="x">The argument.</param>
    /// <returns>The evaluated value.</returns>
    public static Double LogGamma(Double x)
    {
        if (x <= 0.0)
        {
            return Double.PositiveInfinity;
        }
        else if (x > 16.0)
        {
            return LogGamma_Stirling(x);
        }

        return LanczosLogGamma(x);
    }

    /// <summary>
    /// Computes the complex (log) gamma function.
    /// </summary>
    /// <param name="z">The complex argument.</param>
    /// <returns>The evaluated value.</returns>
		public static Complex LogGamma(Complex z)
    {
        if (z.Real < 0.0)
        {
            return new Complex(Double.PositiveInfinity, 0.0);
        }

        if (Complex.Abs(z) > 15.0)
        {
            return LogGamma_Stirling(z);
        }

        return LanczosLogGamma(z);
    }

    #endregion

    #region Beta

    /// <summary>
    /// Computes the real beta function, Gamma(a) * Gamma(b) / Gamma(a+b).
    /// </summary>
    /// <param name="a">The first parameter.</param>
    /// <param name="b">The second parameter.</param>
    /// <returns>The evaluated value.</returns>
    public static Double Beta(Double a, Double b) => Math.Exp(LogGamma(a) + LogGamma(b) - LogGamma(a + b));

    /// <summary>
    /// Computes the complex beta function, Gamma(a) * Gamma(b) / Gamma(a+b).
    /// </summary>
    /// <param name="a">The first complex parameter.</param>
    /// <param name="b">The second complex parameter.</param>
    /// <returns>The evaluated value.</returns>
    public static Complex Beta(Complex a, Complex b) => Complex.Exp(LogGamma(a) + LogGamma(b) - LogGamma(a + b));

    #endregion

    #region Psi

    /// <summary>
    /// Computes the real psi, usually called the digamma function, which is defined as the logarithmic derivative of the gamma function.
    /// </summary>
    /// <param name="x">The real argument.</param>
    /// <returns>The value.</returns>
    public static Double Psi(Double x)
    {
        if (x <= 0.0)
        {
            if (x == Math.Ceiling(x))
            {
                return Double.NaN;
            }

            return Psi(1.0 - x) - Math.PI / Math.Tan(Math.PI * x);
        }
        else if (x > 16.0)
        {
            return Psi_Stirling(x);
        }

        return LanczosPsi(x);
    }

    #endregion

    #region Helpers

    private static Double LogGamma_Stirling(Double x)
    {
        var f = (x - 0.5) * Math.Log(x) - x + Math.Log(2.0 * Math.PI) / 2.0;
        var xsqu = x * x;
        var xp = x;

        for (int i = 1; i < 10; i++)
        {
            var f_old = f;
            f += Mathx.BernoulliNumbers[i] / (2 * i) / (2 * i - 1) / xp;

            if (f == f_old)
            {
                return (f);
            }

            xp *= xsqu;
        }

        throw new Exception("gamma not converged");
    }

    private static Complex LogGamma_Stirling(Complex z)
    {
        if (z.Imaginary < 0.0)
        {
            return Complex.Conjugate(LogGamma_Stirling(Complex.Conjugate(z)));
        }

        var f = (z - 0.5) * Complex.Log(z) - z + Math.Log(2.0 * Math.PI) / 2.0;
        var reduce = f.Imaginary / (2.0 * Math.PI);
        reduce = f.Imaginary - (int)(reduce) * 2.0 * Math.PI;
        f = new Complex(f.Real, reduce);

        var zsqu = z * z;
        var zp = new Complex(z.Real, z.Imaginary);

        for (var i = 1; i < 10; i++)
        {
            var f_old = new Complex(f.Real, f.Imaginary);
            f += Mathx.BernoulliNumbers[i] / (2 * i) / (2 * i - 1) / zp;

            if (f == f_old)
            {
                return (f);
            }

            zp *= zsqu;
        }

        throw new Exception("gamma not converged");
    }

    private static double LanczosLogGamma(double x)
    {
        var sum = Mathx.LanczosD[0];

        for (var i = 1; i < Mathx.LanczosD.Length; i++)
        {
            sum += Mathx.LanczosD[i] / (x + i);
        }

        sum = 2.0 / Math.Sqrt(Math.PI) * sum / x;
        var xshift = x + 0.5;
        var t = xshift * Math.Log(xshift + Mathx.LanczosR) - x;

        return t + Math.Log(sum);
    }

    private static Complex LanczosLogGamma(Complex z)
    {
        var sum = new Complex(Mathx.LanczosD[0], 0.0);

        for (int i = 1; i < Mathx.LanczosD.Length; i++)
        {
            sum += Mathx.LanczosD[i] / (z + i);
        }

        sum = (2.0 / Math.Sqrt(Math.PI)) * (sum / z);
        var zshift = z + 0.5;
        var t = zshift * Complex.Log(zshift + Mathx.LanczosR) - z;

        return t + Complex.Log(sum);
    }

    private static double LanczosPsi(double x)
    {
        var s0 = Mathx.LanczosD[0];
        var s1 = 0.0;

        for (int i = 1; i < Mathx.LanczosD.Length; i++)
        {
            var xi = x + i;
            var st = Mathx.LanczosD[i] / xi;
            s0 += st;
            s1 += st / xi;
        }

        var xx = x + Mathx.LanczosR + 0.5;
        var t = Math.Log(xx) - Mathx.LanczosR / xx - 1.0 / x;

        return (t - s1 / s0);
    }

    private static Double Psi_Stirling(Double x)
    {
        var f = Math.Log(x) - 1.0 / (2.0 * x);
        var xsqu = x * x;
        var xp = xsqu;

        for (int i = 1; i < 10; i++)
        {
            var f_old = f;
            f -= Mathx.BernoulliNumbers[i] / (2 * i) / xp;

            if (f == f_old)
            {
                return (f);
            }

            xp *= xsqu;
        }

        throw new Exception("gamma not converged");
    }

    #endregion
}
