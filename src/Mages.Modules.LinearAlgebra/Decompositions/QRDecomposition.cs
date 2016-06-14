namespace Mages.Modules.LinearAlgebra.Decompositions
{
    using System;

    /// <summary>
    /// QR Decomposition.
    /// For an m-by-n matrix A with m >= n, the QR decomposition is an m-by-n
    /// orthogonal matrix Q and an n-by-n upper triangular matrix R so that
    /// A = Q * R.
    /// The QR decompostion always exists, even if the matrix does not have
    /// full rank, so the constructor will never fail.  The primary use of the
    /// QR decomposition is in the least squares solution of nonsquare systems
    /// of simultaneous linear equations.  This will fail if IsFullRank()
    /// returns false.
    /// </summary>
    public abstract class QRDecomposition : IDirectSolver
    {
        #region Fields
        
        protected readonly Int32 _rows;
        protected readonly Int32 _columns;

        #endregion

        #region ctor

        /// <summary>
        /// QR Decomposition, computed by Householder reflections.
        /// </summary>
        /// <param name="A">Rectangular matrix</param>
        /// <returns>Structure to access R and the Householder vectors and compute Q.</returns>
        protected QRDecomposition(Double[,] A)
        {
            // Initialize.
            HasFullRank = true;
            _rows = A.GetLength(0);
            _columns = A.GetLength(1);
        }

        /// <summary>
        /// Creates the right QR decomposition (Givens or Householder) depending on the given matrix.
        /// </summary>
        /// <param name="A">The matrix to decompose.</param>
        /// <returns>The right QR decomposition implementation.</returns>
        public static QRDecomposition Create(Double[,] A)
        {
            //if (A.IsComplex)
            //    return new GivensDecomposition(A);

            return new HouseholderDecomposition(A);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the matrix has full rank.
        /// </summary>
        public Boolean HasFullRank
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the upper triangular factor.
        /// </summary>
        public abstract Double[,] R
        {
            get;
        }

        /// <summary>
        /// Gets the (economy-sized) orthogonal factor.
        /// </summary>
        public abstract Double[,] Q
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Least squares solution of A * X = B
        /// </summary>
        /// <param name="b">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        public abstract Double[,] Solve(Double[,] b);

        #endregion
    }
}