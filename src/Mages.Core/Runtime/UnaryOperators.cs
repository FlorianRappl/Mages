namespace Mages.Core.Runtime;

using Mages.Core.Runtime.Converters;
using Mages.Core.Runtime.Functions;
using System;
using System.Numerics;

static class UnaryOperators
{
    #region Not Fields

    private static readonly Func<Complex, Object> NotComplex = x => x == Complex.Zero;
    private static readonly Func<Double, Object> NotNumber = x => x == 0.0;
    private static readonly Func<Double[,], Object> NotMatrix = x => x.AreEqual(0.0);
    private static readonly Func<Complex[,], Object> NotCMatrix = x => x.AreEqual(0.0);
    private static readonly Func<Object, Object> NotAny = x => !x.ToBoolean();

    #endregion

    #region Positive Fields

    private static readonly Func<Double, Object> PositiveNumber = m => m;
    private static readonly Func<Complex, Object> PositiveComplex = m => m;
    private static readonly Func<Double[,], Object> PositiveMatrix = m => m;
    private static readonly Func<Complex[,], Object> PositiveCMatrix = m => m;
    private static readonly Func<Object, Object> PositiveAny = m => +m.ToNumber();

    #endregion

    #region Negative Fields

    private static readonly Func<Double, Object> NegativeNumber = m => -m;
    private static readonly Func<Complex, Object> NegativeComplex = m => -m;
    private static readonly Func<Double[,], Object> NegativeMatrix = m => m.ForEach(x => -x);
    private static readonly Func<Complex[,], Object> NegativeCMatrix = m => m.ForEach(x => -x);
    private static readonly Func<Object, Object> NegativeAny = m => -m.ToNumber();

    #endregion

    #region Factorial Fields

    private static readonly Func<Double, Object> FactorialNumber = x => Mathx.Factorial(x);
    private static readonly Func<Complex, Object> FactorialComplex = x => Mathx.Factorial(x);
    private static readonly Func<Double[,], Object> FactorialMatrix = x => x.ForEach(y => Mathx.Factorial(y));
    private static readonly Func<Complex[,], Object> FactorialCMatrix = x => x.ForEach(y => Mathx.Factorial(y));
    private static readonly Func<Object, Object> FactorialAny = x => Mathx.Factorial(x.ToNumber());

    #endregion

    #region Other Fields

    private static readonly Func<Double, Object> NumberToMatrix = x => x.ToMatrix();
    private static readonly Func<Complex, Object> CNumberToMatrix = x => x.ToMatrix();
    private static readonly Func<Double[,], Object> TransposeMatrix = x => x.Transpose();
    private static readonly Func<Complex[,], Object> TransposeCMatrix = x => x.Transpose();

    #endregion

    #region Abs

    private static readonly Func<Double, Object> AbsNumber = x => Math.Abs(x);
    private static readonly Func<Complex, Object> AbsCNumber = x => Complex.Abs(x);
    private static readonly Func<Double[,], Object> AbsMatrix = x => Matrix.Abs(x);
    private static readonly Func<Complex[,], Object> AbsCMatrix = x => Matrix.Abs(x);

    #endregion

    #region Functions

    public static Object Not(Object[] args) =>
        If.Is<Double>(args, NotNumber) ??
        If.Is<Complex>(args, NotComplex) ??
        If.Is<Double[,]>(args, NotMatrix) ??
        If.Is<Complex[,]>(args, NotCMatrix) ??
        If.Is<Object>(args, NotAny) ??
        true;

    public static Object Positive(Object[] args) =>
        If.Is<Double>(args, PositiveNumber) ??
        If.Is<Complex>(args, PositiveComplex) ??
        If.Is<Double[,]>(args, PositiveMatrix) ??
        If.Is<Complex[,]>(args, PositiveCMatrix) ??
        If.Is<Object>(args, PositiveAny) ??
        Double.NaN;

    public static Object Negative(Object[] args) =>
        If.Is<Double>(args, NegativeNumber) ??
        If.Is<Complex>(args, NegativeComplex) ??
        If.Is<Double[,]>(args, NegativeMatrix) ??
        If.Is<Complex[,]>(args, NegativeCMatrix) ??
        If.Is<Object>(args, NegativeAny) ??
        Double.NaN;

    public static Object Factorial(Object[] args) =>
        If.Is<Double>(args, FactorialNumber) ??
        If.Is<Complex>(args, FactorialComplex) ??
        If.Is<Double[,]>(args, FactorialMatrix) ??
        If.Is<Complex[,]>(args, FactorialCMatrix) ??
        If.Is<Object>(args, FactorialAny) ??
        Double.NaN;

    public static Object Transpose(Object[] args) =>
        If.Is<Double[,]>(args, TransposeMatrix) ??
        If.Is<Complex[,]>(args, TransposeCMatrix) ??
        If.Is<Double>(args, NumberToMatrix) ??
        If.Is<Complex>(args, CNumberToMatrix) ??
        args[0].ToNumber().ToMatrix();

    public static Object Abs(Object[] args) =>
        If.Is<Double>(args, AbsNumber) ??
        If.Is<Complex>(args, AbsCNumber) ??
        If.Is<Double[,]>(args, AbsMatrix) ??
        If.Is<Complex[,]>(args, AbsCMatrix) ??
        Math.Abs(args[0].ToNumber());

    public static Object Type(Object[] args) => args[0].ToType();

    #endregion
}
