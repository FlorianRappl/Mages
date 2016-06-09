namespace Mages.Modules.LinearAlgebra.Decompositions
{
    using System;

    /// <summary>
    /// Cholesky Decomposition.
    /// For a symmetric, positive definite matrix A, the Cholesky decomposition
    /// is an lower triangular matrix L so that A = L*L'.
    /// If the matrix is not symmetric or positive definite, the constructor
    /// returns a partial decomposition and sets an internal flag that may
    /// be queried by the isSPD() method.
    /// </summary>
    public class CholeskyDecomposition : IDirectSolver
    {
        #region Fields
        
        private readonly Double[,] _L;
        private readonly Int32 _dim;
        private readonly Boolean _spd;

        #endregion 

        #region ctor

        /// <summary>
        /// Cholesky algorithm for symmetric and positive definite matrix.
        /// </summary>
        /// <param name="matrix">Square, symmetric matrix.</param>
        /// <returns>Structure to access L and isspd flag.</returns>
        public CholeskyDecomposition(Double[,] matrix)
        {
            // Initialize.
            var A = (Double[,])matrix.Clone();
            _dim = matrix.GetLength(0);
            _L = new Double[_dim, _dim];
            _spd = matrix.GetLength(1) == _dim;

            // Main loop.
            for (var i = 0; i < _dim; i++)
            {
                var Lrowi = new Double[_dim];
                var d = 0.0;

                for (var k = 0; k < _dim; k++)
                {
                    Lrowi[k] =_L[i, k];
                }

                for (int j = 0; j < i; j++)
                {
                    var Lrowj = new Double[_dim];
                    var s = 0.0;

                    for (var k = 0; k < _dim; k++)
                    {
                        Lrowj[k] = _L[j, k];
                    }

                    for (var k = 0; k < j; k++)
                    {
                        s += Lrowi[k] * Lrowj[k];
                    }

                    s = (A[i, j] - s) / _L[j, j];
                    Lrowi[j] = s;
                    d += s * s;
                    _spd = _spd && (A[j, i] == A[i, j]);
                }

                d = A[i, i] - d;
                _spd = _spd & (Math.Abs(d) > 0.0);
                _L[i, i] = Math.Sqrt(d);

                for (var k = i + 1; k < _dim; k++)
                {
                    _L[i, k] = 0.0;
                }
            }
        }

        #endregion //  Constructor

        #region Properties

        /// <summary>
        /// Gets if the matrix is symmetric and positive definite.
        /// </summary>
        public Boolean IsSpd
        {
            get { return _spd; }
        }

        /// <summary>
        /// Gets the triangular factor.
        /// </summary>

        public Double[,] L
        {
            get { return _L; }
        }

        #endregion

        #region Methods

        /// <summary>Solve A*X = B</summary>
        /// <param name="matrix">  A Matrix with as many rows as A and any number of columns.
        /// </param>

        public Double[,] Solve(Double[,] matrix)
        {
            if (matrix.GetLength(0) != _dim)
                throw new InvalidOperationException(ErrorMessages.DimensionMismatch);

            if (!_spd)
                throw new InvalidOperationException(ErrorMessages.SpdRequired);

            // Copy right hand side.
            var X = (Double[,])matrix.Clone();
            var nx = matrix.GetLength(1);

            // Solve L*Y = B;
            for (var k = 0; k < _dim; k++)
            {
                for (var i = k + 1; i < _dim; i++)
                {
                    for (var j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * _L[i, k];
                    }
                }

                for (var j = 0; j < nx; j++)
                {
                    X[k, j] /= _L[k, k];
                }
            }

            // Solve L'*X = Y;
            for (var k = _dim - 1; k >= 0; k--)
            {
                for (var j = 0; j < nx; j++)
                {
                    X[k, j] /= _L[k, k];
                }

                for (var i = 0; i < k; i++)
                {
                    for (var j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * _L[k, i];
                    }
                }
            }

            return X;
        }

        #endregion
    }
}