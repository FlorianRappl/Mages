namespace Mages.Modules.LinearAlgebra.Decompositions
{
    using System;

    /// <summary>
    /// The Givens rotation is an implementation of a QR decomposition.
    /// This decomposition also works for complex numbers.
    /// </summary>
    public class GivensDecomposition : QRDecomposition, IDirectSolver
    {
        #region Fields

        private readonly Double[,] q;
        private readonly Double[,] r;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new Givens decomposition.
        /// </summary>
        /// <param name="matrix">The matrix to decompose.</param>
        public GivensDecomposition(Double[,] matrix)
            : base(matrix)
        {
            var Q = Helpers.One(_rows);
            var R = (Double[,])matrix.Clone();

            // Main loop.
            for (var j = 0; j < _columns - 1; j++)
            {
                for (var i = _rows - 1; i > j; i--)
                {
                    var a = R[i - 1, j];
                    var b = R[i, j];
                    var G = Helpers.One(_rows);

                    var beta = Math.Sqrt(a * a + b * b);
                    var s = -b / beta;
                    var c = a / beta;

                    G[i - 1, i - 1] = c;
                    G[i - 1, i] = -s;
                    G[i, i - 1] = s;
                    G[i, i] = c;

                    R = Helpers.Multiply(G, R);
                    Q = Helpers.Multiply(Q, Helpers.Transpose(G));
                }
            }

            for (var j = 0; j < _columns; j++)
            {
                if (R[j, j] == 0.0)
                {
                    HasFullRank = false;
                }
            }

            r = R;
            q = Q;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the (economy-sized) orthogonal factor.
        /// </summary>
        public override Double[,] Q
        {
            get { return q; }
        }

        /// <summary>
        /// Gets the upper triangular factor.
        /// </summary>
        public override Double[,] R
        {
            get { return r; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Least squares solution of A * X = B
        /// </summary>
        /// <param name="b">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        public Double[,] Solve(Double[,] b)
        {
            if (b.GetLength(0) != _rows)
                throw new InvalidOperationException(ErrorMessages.RowMismatch);

            if (!HasFullRank)
                throw new InvalidOperationException(ErrorMessages.SingularSource);

            var cols = b.GetLength(1);

            var r = Helpers.Multiply(Helpers.Transpose(Q), b);
            var x = new Double[_columns, cols];

            for (var j = _columns - 1; j >= 0; j--)
            {
                for (var i = 0; i < cols; i++)
                {
                    var o = 0.0;

                    for (var k = j; k < _columns; k++)
                    {
                        o += R[j, k] * x[k, i];
                    }

                    x[j, i] = (r[j, i] - o) / R[j, j];   
                }
            }

            return x;
        }

        #endregion
    }
}
