namespace Mages.Modules.LinearAlgebra.Solvers
{
    using System;

    /// <summary>
    /// Basic class for a GMRES(k) (with restarts) solver.
    /// </summary>
    public class GMRESkSolver : IIterativeSolver
    {
        #region Fields

        private readonly Double[,] _A;

        #endregion

        #region ctor

        /// <summary>
        /// Creates the class for a GMRES(k) solver.
        /// </summary>
        /// <param name="A">The matrix A to solve.</param>
        public GMRESkSolver(Double[,] A) : 
            this(A, false)
        {
        }

        /// <summary>
        /// Creates the class for a GMRES(k) solver.
        /// </summary>
        /// <param name="A">The matrix A to consider as system of linear equations.</param>
        /// <param name="restart">Should restarts be executed?</param>
        public GMRESkSolver(Double[,] A, Boolean restart)
        {
            MaxIterations = A.Length;
            _A = A;
            Tolerance = 1e-10;
            Restart = MaxIterations / (restart ? 10 : 1);
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
        public Int32 MaxIterations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the starting vector x0.
        /// </summary>
        public Double[,] Guess
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tolerance level (when to stop the iteration?).
        /// </summary>
        public Double Tolerance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if restarts should be performed.
        /// </summary>
        public Int32 Restart
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Solves the system of linear equations.
        /// </summary>
        /// <param name="b">The vector b in A * x = b.</param>
        /// <returns>The solution vector x.</returns>
        public Double[,] Solve(Double[,] b)
        {
            var k = Restart;
            var x = default(Double[,]);
            var converged = false;
            var c = new Double[k - 1];
            var s = new Double[k - 1];
            var gamma = new Double[k + 1];
            var iter = 0;

            if (Guess == null)
            {
                Guess = new Double[b.GetLength(0), b.GetLength(1)];
            }

            if (Guess.GetLength(0) != b.GetLength(0) || Guess.GetLength(1) != b.GetLength(1))
                throw new InvalidOperationException(ErrorMessages.DimensionMismatch);

            var H = new Double[k + 1, k];
            var V = new Double[Guess.GetLength(0), k];

            do
            {
                var j = 0;
                x = (Double[,])Guess.Clone();
                var r0 = Helpers.Subtract(b, Helpers.Multiply(Matrix, x));
                var beta = Helpers.Norm(r0);

                H.Initialize();
                V.Initialize();
                gamma.Initialize();
                c.Initialize();
                s.Initialize();

                gamma[0] = beta;
                
                Helpers.SetColumnVector(V, 1, Helpers.Multiply(r0, 1.0 / beta));

                if (beta < Tolerance)
                {
                    break;
                }

                do
                {
                    iter++;

                    var Avj = Helpers.Multiply(Matrix, Helpers.GetColumnVector(V, j));
                    var sum = new Double[Avj.GetLength(0), Avj.GetLength(1)];

                    for (var m = 0; m <= j; m++)
                    {
                        var w = Helpers.GetColumnVector(V, m);
                        H[m, j] = Helpers.Reduce(w, Avj);
                        sum = Helpers.AddScaled(sum, H[m, j], w);
                    }

                    var wj = Helpers.Subtract(Avj, sum);
                    H[j + 1, j] = Helpers.Reduce(wj, wj);
                    Rotate(j, H, c, s, gamma);

                    if (Math.Abs(H[j + 1, j]) == 0.0)
                    {
                        j++;
                        converged = true;
                        break;
                    }

                    Helpers.SetColumnVector(V, j + 1, Helpers.Multiply(wj, 1.0 / H[j + 1, j]));
                    beta = Math.Abs(gamma[j]);

                    if (beta < Tolerance)
                    {
                        j++;
                        converged = true;
                        break;
                    }

                    j++;
                }
                while (j < k);

                var y = new Double[j];

                for (var l = j; l >= 1; l--)
                {
                    var sum = 0.0;

                    for (var m = l + 1; m <= j; m++)
                    {
                        sum += H[l, m] * y[m];
                    }

                    y[l] = (gamma[l] - sum) / H[l, l];
                }

                for (var l = 0; l < j; l++)
                {
                    x = Helpers.AddScaled(x, y[l], Helpers.GetColumnVector(V, l));
                }

                if (converged)
                {
                    break;
                }

                Guess = x;
            }
            while (iter < MaxIterations);

            return x;
        }

        private void Rotate(Int32 j, Double[,] H, Double[] c, Double[] s, Double[] gamma)
        {
            for (var i = 0; i < j; i++)
            {
                var v1 = H[i, j];
                var v2 = H[i + 1, j];
                H[i, j] = c[i] * v1 + s[i] * v2;
                H[i + 1, j] = c[i] * v2 - s[i] * v1;
            }

            var beta = Math.Sqrt(Math.Abs(H[j, j])) + Math.Sqrt(Math.Abs(H[j + 1, j]));

            s[j] = H[j + 1, j] / beta;
            c[j] = H[j, j] / beta;
            H[j, j] = beta;

            gamma[j + 1] = -s[j] * gamma[j];
            gamma[j] = c[j] * gamma[j];
        }

        #endregion
    }
}
