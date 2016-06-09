namespace Mages.Modules.LinearAlgebra.Decompositions
{
    using System;

    /// <summary>
    /// The Householder reflection is an implementation of a QR decomposition.
    /// This decomposition does not work for complex numbers.
    /// </summary>
    public class HouseholderDecomposition : QRDecomposition, IDirectSolver
    {
        #region Fields
        
        private readonly Double[] _Rdiag;
        private readonly Double[,] _QR;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new householder decomposition.
        /// </summary>
        /// <param name="matrix">The matrix to decompose.</param>
        public HouseholderDecomposition(Double[,] matrix)
            : base(matrix)
        {
            _QR = (Double[,])matrix.Clone();
            _Rdiag = new Double[_columns];

            // Main loop.
            for (int k = 0; k < _columns; k++)
            {
                var nrm = 0.0;

                for (int i = k; i < _rows; i++)
                {
                    nrm = Helpers.Hypot(nrm, _QR[i, k]);
                }

                if (nrm != 0.0)
                {
                    // Form k-th Householder vector.

                    if (_QR[k, k] < 0.0)
                    {
                        nrm = -nrm;
                    }

                    for (var i = k; i < _rows; i++)
                    {
                        _QR[i, k] /= nrm;
                    }

                    _QR[k, k] += 1.0;

                    // Apply transformation to remaining columns.
                    for (var j = k + 1; j < _columns; j++)
                    {
                        var s = 0.0;

                        for (var i = k; i < _rows; i++)
                        {
                            s += _QR[i, k] * _QR[i, j];
                        }

                        s = (-s) / _QR[k, k];

                        for (var i = k; i < _rows; i++)
                        {
                            _QR[i, j] += s * _QR[i, k];
                        }
                    }
                }
                else
                {
                    HasFullRank = false;
                }

                _Rdiag[k] = -nrm;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the upper triangular factor.
        /// </summary>
        public override Double[,] R
        {
            get
            {
                var X = new Double[_columns, _columns];

                for (var i = 0; i < _columns; i++)
                {
                    for (var j = 0; j < _columns; j++)
                    {
                        if (i < j)
                        {
                            X[i, j] = _QR[i, j];
                        }
                        else if (i == j)
                        {
                            X[i, j] = _Rdiag[i];
                        }
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Gets the (economy-sized) orthogonal factor.
        /// </summary>
        public override Double[,] Q
        {
            get
            {
                var X = new Double[_rows, _columns];

                for (var k = _columns - 1; k >= 0; k--)
                {
                    for (var i = 0; i < _rows; i++)
                    {
                        X[i, k] = 0.0;
                    }

                    X[k, k] = 1.0;

                    for (var j = k; j < _columns; j++)
                    {
                        if (_QR[k, k] != 0)
                        {
                            var s = 0.0;

                            for (var i = k; i < _rows; i++)
                            {
                                s += _QR[i, k] * X[i, j];
                            }

                            s = (-s) / _QR[k, k];

                            for (var i = k; i < _rows; i++)
                            {
                                X[i, j] = X[i, j] + s * _QR[i, k];
                            }
                        }
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Gets the Householder vectors.
        /// </summary>
        public Double[,] H
        {
            get
            {
                var X = new Double[_rows, _columns];

                for (var i = 0; i < _rows; i++)
                {
                    for (var j = 0; j < _columns; j++)
                    {
                        if (i >= j)
                        {
                            X[i, j] = _QR[i, j];
                        }
                    }
                }

                return X;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Least squares solution of A * X = B
        /// </summary>
        /// <param name="matrix">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        public Double[,] Solve(Double[,] matrix)
        {
            if (matrix.GetLength(0) != _rows)
                throw new InvalidOperationException(ErrorMessages.RowMismatch);

            if (!HasFullRank)
                throw new InvalidOperationException(ErrorMessages.SingularSource);

            // Copy right hand side
            var nx = matrix.GetLength(1);
            var X = (Double[,])matrix.Clone();

            // Compute Y = transpose(Q)*B
            for (var k = 0; k < _columns; k++)
            {
                for (var j = 0; j < nx; j++)
                {
                    var s = 0.0;

                    for (var i = k; i < _rows; i++)
                    {
                        s += _QR[i, k] * X[i, j];
                    }

                    s = (-s) / _QR[k, k];

                    for (var i = k; i < _rows; i++)
                    {
                        X[i, j] += s * _QR[i, k];
                    }
                }
            }

            // Solve R * X = Y;
            for (var k = _columns - 1; k >= 0; k--)
            {
                for (var j = 0; j < nx; j++)
                {
                    X[k, j] /= _Rdiag[k];
                }

                for (var i = 0; i < k; i++)
                {
                    for (var j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * _QR[i, k];
                    }
                }
            }

            return Helpers.SubMatrix(X, 0, _columns, 0, nx);
        }

        #endregion
    }
}
