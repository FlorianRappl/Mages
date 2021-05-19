namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Numerics;

    /// <summary>
    /// The collection of all standard complex functions.
    /// </summary>
    public static class ComplexFunctions
    {
        /// <summary>
        /// Exposes the complex constructor function.
        /// </summary>
        public static readonly Function Cmplx = new Function(args => 
            Curry.MinTwo(Cmplx, args) ??
            If.Is<Double, Double>(args, (x, y) => new Complex(x, y)) ??
            Complex.Zero
        );

        /// <summary>
        /// Exposes the complex conjugate function.
        /// </summary>
        public static readonly Function Conj = new Function(args =>
            Curry.MinOne(Conj, args) ??
            If.Is<Double>(args, x => new Complex(x, 0)) ??
            If.Is<Complex>(args, x => new Complex(x.Real, -x.Imaginary)) ??
            Double.NaN
        );

        /// <summary>
        /// Exposes the complex real function.
        /// </summary>
        public static readonly Function Real = new Function(args =>
            Curry.MinOne(Real, args) ??
            If.Is<Double>(args, x => x) ??
            If.Is<Complex>(args, x => x.Real) ??
            Double.NaN
        );

        /// <summary>
        /// Exposes the complex imag function.
        /// </summary>
        public static readonly Function Imag = new Function(args =>
            Curry.MinOne(Imag, args) ??
            If.Is<Double>(args, _ => 0.0) ??
            If.Is<Complex>(args, x => x.Imaginary) ??
            Double.NaN
        );
    }
}
