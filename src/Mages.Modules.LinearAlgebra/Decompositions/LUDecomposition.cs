namespace Mages.Modules.LinearAlgebra.Decompositions
{
    using System;

    /// <summary>
    /// LU Decomposition.
    /// For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    /// unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    /// and a permutation vector piv of length m so that A(piv,:) = L*U.
    /// <code>
    /// If m is smaller than n, then L is m-by-m and U is m-by-n.
    /// </code>
    /// The LU decompostion with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail.  The primary use of the
    /// LU decomposition is in the solution of square systems of simultaneous
    /// linear equations. This will fail if IsNonSingular() returns false.
    /// </summary>
    public class LUDecomposition : IDirectSolver
    {
        #region Fields

        private readonly Double[,] _LU;
        private readonly Int32 _rows;
        private readonly Int32 _columns;
        private readonly Int32 _pivsign;
        private readonly Int32[] _piv;

        #endregion

        #region ctor

        /// <summary>
        /// LU Decomposition
        /// </summary>
        /// <param name="matrix">Rectangular matrix</param>
        /// <returns>Structure to access L, U and piv.</returns>
        public LUDecomposition(Double[,] matrix)
        {
            // Use a "left-looking", dot-product, Crout / Doolittle algorithm.
            _LU = (Double[,])matrix.Clone();
            _rows = matrix.GetLength(0);
            _columns = matrix.GetLength(1);
            _piv = new Int32[_rows];

            for (var i = 1; i < _rows; i++)
            {
                _piv[i] = i;
            }
            
            _pivsign = 1;
            var LUcolj = new Double[_rows];

            // Outer loop.
            for (var j = 0; j < _columns; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (var i = 0; i < _rows; i++)
                {
                    LUcolj[i] = _LU[i, j];
                }

                // Apply previous transformations.
                for (var i = 0; i < _rows; i++)
                {
                    var LUrowi = new Double[_columns];

                    for (var k = 0; k < _columns; k++)
                    {
                        LUrowi[k] = _LU[i, k];
                    }

                    // Most of the time is spent in the following dot product.
                    var kmax = Math.Min(i, j);
                    var s = 0.0;

                    for (var k = 0; k < kmax; k++)
                    {
                        s += LUrowi[k] * LUcolj[k];
                    }

					LUrowi[j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                var p = j;

                for (var i = j + 1; i < _rows; i++)
                {
                    if (Math.Abs(LUcolj[i]) > Math.Abs(LUcolj[p]))
                    {
                        p = i;
                    }
                }

                if (p != j)
                {
                    for (var k = 0; k < _columns; k++)
                    {
                        var t = _LU[p, k]; 
                        _LU[p, k] = _LU[j, k]; 
                        _LU[j, k] = t;
                    }
                    
                    var k2 = _piv[p];
                    _piv[p] = _piv[j];
                    _piv[j] = k2;
                    _pivsign = -_pivsign;
                }

                // Compute multipliers.

                if (j < _rows & _LU[j, j] != 0.0)
                {
                    for (var i = j + 1; i < _rows; i++)
                    {
                        _LU[i, j] = _LU[i, j] / _LU[j, j];
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the matrix nonsingular.
        /// </summary>
        public Boolean IsNonSingular
        {
            get
            {
                for (var j = 0; j < _columns; j++)
                {
                    if (_LU[j, j] == 0.0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the lower triangular factor.
        /// </summary>
        public Double[,] L
        {
            get
            {
                var X = new Double[_rows, _columns];

                for (var i = 0; i < _rows; i++)
                {
                    for (var j = 0; j < _columns; j++)
                    {
                        if (i > j)
                        {
                            X[i, j] = _LU[i, j];
                        }
                        else if (i == j)
                        {
                            X[i, j] = 1.0;
                        }
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Gets the upper triangular factor.
        /// </summary>
        public Double[,] U
        {
            get
            {
                var X = new Double[_columns, _columns];

                for (var i = 0; i < _columns; i++)
                {
                    for (var j = 0; j < _columns; j++)
                    {
                        if (i <= j)
                        {
                            X[i, j] = _LU[i, j];
                        }
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Gets the pivot permutation vector.
        /// </summary>
        public Double[,] Pivot
        {
            get
            {
				var P = new Double[_rows, _rows];

                for (var i = 0; i < _rows; i++)
                {
                    P[i, _piv[i]] = 1.0;
                }
                
                return P;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the determinant of the given matrix.
        /// </summary>
        public static Double Determinant(Double[,] matrix)
        {
            var lu = new LUDecomposition(matrix);

            if (lu._rows == lu._columns)
            {
                var d = (Double)lu._pivsign;

                for (var j = 0; j < lu._columns; j++)
                {
                    d = d * lu._LU[j, j];
                }

                return d;
            }

            return 0.0;
        }

        /// <summary>
        /// Solve A*X = B
        /// </summary>
        /// <param name="matrix">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X so that L*U*X = B(piv,:)</returns>
        public Double[,] Solve(Double[,] matrix)
        {
            if (matrix.GetLength(0) != _rows)
                throw new InvalidOperationException(ErrorMessages.RowMismatch);

            if (!IsNonSingular)
                throw new InvalidOperationException(ErrorMessages.SingularSource);

            // Copy right hand side with pivoting
            var nx = matrix.GetLength(1);
            var X = Helpers.SubMatrix(matrix, _piv, 0, nx);

            // Solve L*Y = B(piv,:)
            for (var k = 0; k < _columns; k++)
            {
                for (var i = k + 1; i < _columns; i++)
                {
                    for (var j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * _LU[i, k];
                    }
                }
            }

            // Solve U*X = Y;
            for (var k = _columns - 1; k >= 0; k--)
            {
                for (var j = 0; j < nx; j++)
                {
                    X[k, j] = X[k, j] / _LU[k, k];
                }

                for (var i = 0; i < k; i++)
                {
                    for (var j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * _LU[i, k];
                    }
                }
            }

			return X;
        }

        #endregion
    }
}