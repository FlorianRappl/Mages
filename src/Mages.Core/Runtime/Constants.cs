namespace Mages.Core.Runtime;

using System;
using System.Numerics;

/// <summary>
/// A set of known / famous / useful constants.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Euler's constant.
    /// The number e is an important mathematical constant, approximately equal to
    /// 2.71828, that is the base of the natural logarithm.
    /// </summary>
    public static Double E = Math.E;

    /// <summary>
    /// Imaginary unit.
    /// </summary>
    public static Complex I = Complex.ImaginaryOne;

    /// <summary>
    /// Imaginary unit (alt).
    /// </summary>
    public static Complex J = Complex.ImaginaryOne;

    /// <summary>
    /// The Feigenbaum constant alpha is the ratio between the width of a tine and the
    /// width of one of its two subtines (except the tine closest to the fold).
    /// </summary>
    public static Double Alpha = 2.50290787509589282228390287321821578;

    /// <summary>
    /// Catalan's constant G, which occasionally appears in estimates in combinatorics,
    /// is defined by G = beta(2), where beta is the Dirichlet beta function.
    /// </summary>
    public static Double Catalan = 0.915965594177219015054604;

    /// <summary>
    /// The Feigenbaum constant delta is the limiting ratio of each bifurcation interval
    /// to the next between every period doubling, of a one-parameter map.
    /// </summary>
    public static Double Delta = 4.66920160910299067185320382046620161;

    /// <summary>
    /// The Euler–Mascheroni constant (also called Euler's constant) is a mathematical
    /// constant recurring in analysis and number theory.
    /// </summary>
    public static Double Gamma1 = 0.57721566490153286060651209008240243;

    /// <summary>
    /// Gauss's constant, denoted by G, is defined as the reciprocal of the
    /// arithmetic-geometric mean of 1 and the square root of 2.
    /// </summary>
    public static Double Gauss = 0.8346268416740731862814297;

    /// <summary>
    /// The omega constant is the value of W(1) where W is Lambert's W function. The name is derived
    /// from the alternate name for Lambert's W function, the omega function.
    /// </summary>
    public static Double Omega = 0.5671432904097838729999686622;

    /// <summary>
    /// The golden ratio: two quantities are in the golden ratio if the ratio of the sum of the
    /// quantities to the larger quantity is equal to the ratio of the larger quantity to the
    /// smaller one.
    /// </summary>
    public static Double Phi = 1.61803398874989484820458683436563811;

    /// <summary>
    /// A degree (in full, a degree of arc, arc degree, or arcdegree), usually denoted by ° (the degree symbol),
    /// is a measurement of plane angle, representing 1⁄360 of a full rotation; one degree is equivalent to
    /// π/180 radians.
    /// </summary>
    public static Double Deg = Math.PI / 180.0;
}
