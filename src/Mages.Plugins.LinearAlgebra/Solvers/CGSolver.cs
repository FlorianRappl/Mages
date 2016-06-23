namespace Mages.Plugins.LinearAlgebra.Solvers
{
    using System;

    /// <summary>
    /// Basic class for a Conjugant Gradient solver.
    /// </summary>
    public class CGSolver : IIterativeSolver
    {
        #region Fields

        private readonly Double[,] _A;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="A">The matrix A for which to solve.</param>
        public CGSolver(Double[,] A)
        {
            _A = A;
            MaxIterations = 5 * A.GetLength(0) * A.GetLength(1);
            Tolerance = 1e-10;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the matrix A in A * x = b.
        /// </summary>
        public Double[,] Matrix
        {
            get { return _A; }
        }

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        public Int32 MaxIterations { get; set; }

        /// <summary>
        /// Gets or sets the starting vector x0.
        /// </summary>
        public Double[,] Guess { get; set; }

        /// <summary>
        /// Gets or sets the tolerance level (when to stop the iteration?).
        /// </summary>
        public Double Tolerance { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Solves a system of linear equation using the given matrix A.
        /// </summary>
        /// <param name="b">The source vector b, i.e. A * x = b.</param>
        /// <returns>The solution vector x.</returns>
        public Double[,] Solve(Double[,] b)
        {
            var x = Guess;

            if (x == null)
            {
                x = new Double[b.GetLength(0), b.GetLength(1)];
            }
            else if (x.GetLength(0) != b.GetLength(0) || x.GetLength(1) != b.GetLength(1))
            {
                throw new InvalidOperationException(ErrorMessages.DimensionMismatch);
            }

            var r = Helpers.Subtract(b, Helpers.Multiply(Matrix, x));
            var p = r;
            var l = Math.Max(Matrix.Length, MaxIterations);
            var rsold = Helpers.Reduce(r, r);

            for (var i = 1; i < l; i++)
            {
                var Ap = Helpers.Multiply(Matrix, p);
                var alpha = rsold / Helpers.Reduce(p, Ap);
                x = Helpers.AddScaled(x, alpha, p);
                r = Helpers.SubtractScaled(r, alpha, Ap);
                var rsnew = Helpers.Reduce(r, r);

                if (Math.Abs(rsnew) < Tolerance)
                {
                    break;
                }

                p = Helpers.AddScaled(r, rsnew / rsold, p);
                rsold = rsnew;
            }

            return x;
        }

        #endregion
    }
}
