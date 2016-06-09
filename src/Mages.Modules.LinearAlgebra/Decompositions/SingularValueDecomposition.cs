namespace Mages.Modules.LinearAlgebra.Decompositions
{
    using System;

    /// <summary>
    /// Singular Value Decomposition.
    /// For an m-by-n matrix A with m >= n, the singular value decomposition is
    /// an m-by-n orthogonal matrix U, an n-by-n diagonal matrix S, and
    /// an n-by-n orthogonal matrix V so that A = U*S*V'.
    /// The singular values, sigma[k] = S[k][k], are ordered so that
    /// sigma[0] >= sigma[1] >= ... >= sigma[n-1].
    /// The singular value decompostion always exists, so the constructor will
    /// never fail.  The matrix condition number and the effective numerical
    /// rank can be computed from this decomposition.
    /// </summary>
    public class SingularValueDecomposition
    {
        #region Fields

        private readonly Double[,] _U;
        private readonly Double[,] _V;
        private readonly Double[] _s;
        private readonly Int32 _rows;
        private readonly Int32 _cols;

        #endregion

        #region ctor

        /// <summary>
        /// Construct the singular value decomposition
        /// </summary>
        /// <param name="Arg">Rectangular matrix</param>
        /// <returns>Structure to access U, S and V.</returns>
        public SingularValueDecomposition(Double[,] matrix)
        {
            var A = matrix;
            _rows = matrix.GetLength(0);
            _cols = matrix.GetLength(1);
            var nu = Math.Min(_rows, _cols);
            _s = new Double[Math.Min(_rows + 1, _cols)];
            _U = new Double[_rows, nu];
            _V = new Double[_cols, _cols];
            var e = new Double[_cols];
            var work = new Double[_rows];
            var wantu = true;
            var wantv = true;

            var nct = Math.Min(_rows - 1, _cols);
            var nrt = Math.Max(0, Math.Min(_cols - 2, _rows));

            for (var k = 0; k < Math.Max(nct, nrt); k++)
            {
                if (k < nct)
                {
                    // Compute the transformation for the k-th column and
                    // place the k-th diagonal in s[k].
                    // Compute 2-norm of k-th column without under/overflow.
                    _s[k] = 0;

                    for (var i = k; i < _rows; i++)
                    {
                        _s[k] = Helpers.Hypot(_s[k], A[i, k]);
                    }
                    
                    if (_s[k] != 0.0)
                    {
                        if (A[k, k] < 0.0)
                        {
                            _s[k] = -_s[k];
                        }

                        for (var i = k; i < _rows; i++)
                        {
                            A[i, k] /= _s[k];
                        }
                        
                        A[k, k] += 1.0;
                    }

                    _s[k] = -_s[k];
                }

                for (var j = k + 1; j < _cols; j++)
                {
                    if ((k < nct) & (_s[k] != 0.0))
                    {
                        // Apply the transformation.
                        var t = 0.0;

                        for (var i = k; i < _rows; i++)
                        {
                            t += A[i, k] * A[i, j];
                        }
                        
                        t = (-t) / A[k, k];

                        for (var i = k; i < _rows; i++)
                        {
                            A[i, j] += t * A[i, k];
                        }
                    }

                    // Place the k-th row of A into e for the
                    // subsequent calculation of the row transformation.
                    e[j] = A[k, j];
                }

                if (wantu & (k < nct))
                {
                    // Place the transformation in U for subsequent back
                    // multiplication.
                    for (var i = k; i < _rows; i++)
                    {
                        _U[i, k] = A[i, k];
                    }
                }

                if (k < nrt)
                {
                    // Compute the k-th row transformation and place the
                    // k-th super-diagonal in e[k].
                    // Compute 2-norm without under/overflow.
                    e[k] = 0;

                    for (var i = k + 1; i < _cols; i++)
                    {
                        e[k] = Helpers.Hypot(e[k], e[i]);
                    }

                    if (e[k] != 0.0)
                    {
                        if (e[k + 1] < 0.0)
                        {
                            e[k] = -e[k];
                        }

                        for (var i = k + 1; i < _cols; i++)
                        {
                            e[i] /= e[k];
                        }
                        
                        e[k + 1] += 1.0;
                    }

                    e[k] = -e[k];

                    if ((k + 1 < _rows) & (e[k] != 0.0))
                    {
                        // Apply the transformation.

                        for (var i = k + 1; i < _rows; i++)
                        {
                            work[i] = 0.0;
                        }
                        
                        for (var j = k + 1; j < _cols; j++)
                        {
                            for (var i = k + 1; i < _rows; i++)
                            {
                                work[i] += e[j] * A[i, j];
                            }
                        }

                        for (var j = k + 1; j < _cols; j++)
                        {
                            var t = (-e[j]) / e[k + 1];

                            for (var i = k + 1; i < _rows; i++)
                            {
                                A[i, j] += t * work[i];
                            }
                        }
                    }

                    if (wantv)
                    {
                        // Place the transformation in V for subsequent
                        // back multiplication.
                        for (var i = k + 1; i < _cols; i++)
                        {
                            _V[i, k] = e[i];
                        }
                    }
                }
            }

            // Set up the final bidiagonal matrix or order p.

            var p = Math.Min(_cols, _rows + 1);

            if (nct < _cols)
            {
                _s[nct] = A[nct, nct];
            }

            if (_rows < p)
            {
                _s[p - 1] = 0.0;
            }

            if (nrt + 1 < p)
            {
                e[nrt] = A[nrt, p - 1];
            }
            
            e[p - 1] = 0.0;

            // If required, generate U.

            if (wantu)
            {
                for (var j = nct; j < nu; j++)
                {
                    for (var i = 0; i < _rows; i++)
                    {
                        _U[i, j] = 0.0;
                    }
                    
                    _U[j, j] = 1.0;
                }

                for (var k = nct - 1; k >= 0; k--)
                {
                    if (_s[k] != 0.0)
                    {
                        for (var j = k + 1; j < nu; j++)
                        {
                            var t = 0.0;

                            for (var i = k; i < _rows; i++)
                            {
                                t += _U[i, k] * _U[i, j];
                            }
                            
                            t = (-t) / _U[k, k];

                            for (var i = k; i < _rows; i++)
                            {
                                _U[i, j] += t * _U[i, k];
                            }
                        }

                        for (var i = k; i < _rows; i++)
                        {
                            _U[i, k] = -_U[i, k];
                        }

                        _U[k, k] = 1.0 + _U[k, k];

                        for (var i = 0; i < k - 1; i++)
                        {
                            _U[i, k] = 0.0;
                        }
                    }
                    else
                    {
                        for (var i = 0; i < _rows; i++)
                        {
                            _U[i, k] = 0.0;
                        }
                        
                        _U[k, k] = 1.0;
                    }
                }
            }

            // If required, generate V.
            if (wantv)
            {
                for (var k = _cols - 1; k >= 0; k--)
                {
                    if ((k < nrt) & (e[k] != 0.0))
                    {
                        for (var j = k + 1; j < nu; j++)
                        {
                            var t = 0.0;

                            for (var i = k + 1; i < _cols; i++)
                            {
                                t += _V[i, k] * _V[i, j];
                            }
                            
                            t = (-t) / _V[k + 1, k];

                            for (var i = k + 1; i < _cols; i++)
                            {
                                _V[i, j] += t * _V[i, k];
                            }
                        }
                    }

                    for (var i = 0; i < _cols; i++)
                    {
                        _V[i, k] = 0.0;
                    }
                    
                    _V[k, k] = 1.0;
                }
            }

            // Main iteration loop for the singular values.
            var pp = p - 1;
            var iter = 0;
            var eps = Math.Pow(2.0, -52.0);

            while (p > 0)
            {
                var k = 0;
                var mode = 0;
                
                // mode = 1     if s(p) and e[k-1] are negligible and k<p
                // mode = 2     if s(k) is negligible and k<p
                // mode = 3     if e[k-1] is negligible, k<p, and s(k), ..., s(p) are not negligible (qr step).
                // mode = 4     if e(p-1) is negligible (convergence).

                for (k = p - 2; k >= -1; k--)
                {
                    if (k == -1)
                        break;
                    
                    if (Math.Abs(e[k]) <= eps * (Math.Abs(_s[k]) + Math.Abs(_s[k + 1])))
                    {
                        e[k] = 0.0;
                        break;
                    }
                }

                if (k == p - 2)
                {
                    mode = 4;
                }
                else
                {
                    var ks = 0;

                    for (ks = p - 1; ks >= k; ks--)
                    {
                        if (ks == k)
                            break;

                        var t = (ks != p ? Math.Abs(e[ks]) : 0.0) + (ks != k + 1 ? Math.Abs(e[ks - 1]) : 0.0);

                        if (Math.Abs(_s[ks]) <= eps * t)
                        {
                            _s[ks] = 0.0;
                            break;
                        }
                    }

                    if (ks == k)
                    {
                        mode = 3;
                    }
                    else if (ks == p - 1)
                    {
                        mode = 1;
                    }
                    else
                    {
                        mode = 2;
                        k = ks;
                    }
                }

                k++;

                // Perform the task indicated by kase.
                switch (mode)
                {
                    // Deflate negligible s(p).
                    case 1:
                        {
                            var f = e[p - 2];
                            e[p - 2] = 0.0;

                            for (var j = p - 2; j >= k; j--)
                            {
                                var t = Helpers.Hypot(_s[j], f);
                                var cs = _s[j] / t;
                                var sn = f / t;
                                _s[j] = t;

                                if (j != k)
                                {
                                    f = (-sn) * e[j - 1];
                                    e[j - 1] = cs * e[j - 1];
                                }

                                if (wantv)
                                {
                                    for (var i = 0; i < _cols; i++)
                                    {
                                        t = cs * _V[i, j] + sn * _V[i, p - 1];
                                        _V[i, p - 1] = (-sn) * _V[i, j] + cs * _V[i, p - 1];
                                        _V[i, j] = t;
                                    }
                                }
                            }
                        }

                        break;

                    // Split at negligible s(k).
                    case 2:
                        {
                            var f = e[k - 1];
                            e[k - 1] = 0.0;

                            for (var j = k; j < p; j++)
                            {
                                var t = Helpers.Hypot(_s[j], f);
                                var cs = _s[j] / t;
                                var sn = f / t;
                                _s[j] = t;
                                f = (-sn) * e[j];
                                e[j] = cs * e[j];

                                if (wantu)
                                {
                                    for (var i = 0; i < _rows; i++)
                                    {
                                        t = cs * _U[i, j] + sn * _U[i, k - 1];
                                        _U[i, k - 1] = (-sn) * _U[i, j] + cs * _U[i, k - 1];
                                        _U[i, j] = t;
                                    }
                                }
                            }
                        }
                        break;

                    // Perform one qr step.
                    case 3:
                        {
                            // Calculate the shift.
                            var scale = Math.Max(Math.Max(Math.Max(Math.Max(Math.Abs(_s[p - 1]), Math.Abs(_s[p - 2])), Math.Abs(e[p - 2])), Math.Abs(_s[k])), Math.Abs(e[k]));
                            var sp = _s[p - 1] / scale;
                            var spm1 = _s[p - 2] / scale;
                            var epm1 = e[p - 2] / scale;
                            var sk = _s[k] / scale;
                            var ek = e[k] / scale;
                            var b = ((spm1 + sp) * (spm1 - sp) + epm1 * epm1) / 2.0;
                            var c = (sp * epm1) * (sp * epm1);
                            var shift = 0.0;

                            if ((b != 0.0) | (c != 0.0))
                            {
                                shift = Math.Sqrt(b * b + c);

                                if (b < 0.0)
                                {
                                    shift = -shift;
                                }
                                
                                shift = c / (b + shift);
                            }

                            var f = (sk + sp) * (sk - sp) + shift;
                            var g = sk * ek;

                            // Chase zeros.

                            for (var j = k; j < p - 1; j++)
                            {
                                var t = Helpers.Hypot(f, g);
                                var cs = f / t;
                                var sn = g / t;

                                if (j != k)
                                {
                                    e[j - 1] = t;
                                }
                                
                                f = cs * _s[j] + sn * e[j];
                                e[j] = cs * e[j] - sn * _s[j];
                                g = sn * _s[j + 1];
                                _s[j + 1] = cs * _s[j + 1];

                                if (wantv)
                                {
                                    for (var i = 0; i < _cols; i++)
                                    {
                                        t = cs * _V[i, j] + sn * _V[i, j + 1];
                                        _V[i, j + 1] = (-sn) * _V[i, j] + cs * _V[i, j + 1];
                                        _V[i, j] = t;
                                    }
                                }

                                t = Helpers.Hypot(f, g);
                                cs = f / t;
                                sn = g / t;
                                _s[j] = t;
                                f = cs * e[j] + sn * _s[j + 1];
                                _s[j + 1] = (-sn) * e[j] + cs * _s[j + 1];
                                g = sn * e[j + 1];
                                e[j + 1] = cs * e[j + 1];

                                if (wantu && (j < _rows - 1))
                                {
                                    for (int i = 0; i < _rows; i++)
                                    {
                                        t = cs * _U[i, j] + sn * _U[i, j + 1];
                                        _U[i, j + 1] = (-sn) * _U[i, j] + cs * _U[i, j + 1];
                                        _U[i, j] = t;
                                    }
                                }
                            }

                            e[p - 2] = f;
                            iter = iter + 1;
                        }
                        break;

                    // Convergence.
                    case 4:
                        {
                            // Make the singular values positive.
                            if (_s[k] <= 0.0)
                            {
                                _s[k] = (_s[k] < 0.0 ? -_s[k] : 0.0);

                                if (wantv)
                                {
                                    for (var i = 0; i <= pp; i++)
                                    {
                                        _V[i, k] = -_V[i, k];
                                    }
                                }
                            }

                            // Order the singular values.
                            while (k < pp)
                            {
                                if (_s[k] >= _s[k + 1])
                                    break;

                                var t = _s[k];
                                _s[k] = _s[k + 1];
                                _s[k + 1] = t;

                                if (wantv && (k < _cols - 1))
                                {
                                    for (var i = 0; i < _cols; i++)
                                    {
                                        t = _V[i, k + 1]; 
                                        _V[i, k + 1] = _V[i, k]; 
                                        _V[i, k] = t;
                                    }
                                }

                                if (wantu && (k < _rows - 1))
                                {
                                    for (var i = 0; i < _rows; i++)
                                    {
                                        t = _U[i, k + 1];
                                        _U[i, k + 1] = _U[i, k]; 
                                        _U[i, k] = t;
                                    }
                                }

                                k++;
                            }

                            iter = 0;
                            p--;
                        }
                        break;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the one-dimensional array of singular values.
        /// </summary>
        public Double[,] SingularValues
        {
            get
            {
                var s = new Double[1, _s.Length];

                for (var i = 0; i < _s.Length; i++)
                {
                    s[0, i] = _s[i];
                }

                return s;
            }
        }

        /// <summary>
        /// Gets the diagonal matrix of singular values.
        /// </summary>
        public Double[,] S
        {
            get
            {
                var X = new Double[_rows, _cols];

                for (var i = 0; i < _rows; i++)
                {
                    X[i, i] = _s[i];
                }

                return X;
            }
        }

        /// <summary>
        /// Gets the left singular vectors.
        /// </summary>
        public Double[,] U
        {
            get { return _U; }
        }

        /// <summary>
        /// Gets the right singular vectors.
        /// </summary>
        public Double[,] V
        {
            get { return _V; }
        }

        /// <summary>
        /// Gets the L2 norm.
        /// </summary>
        public Double Norm2
        {
            get { return _s[0]; }
        }

        /// <summary>
        /// Gets the L2 norm condition number.
        /// </summary>
        public Double Condition
        {
            get { return _s[0] / _s[Math.Min(_rows, _cols) - 1]; }
        }

        #endregion

        #region	Methods

        /// <summary>
        /// Effective numerical matrix rank
        /// </summary>
        /// <returns>Number of nonnegligible singular values.</returns>
        public Int32 ComputeRank()
        {
            var eps = Math.Pow(2.0, -52.0);
            var tol = Math.Max(_rows, _cols) * _s[0] * eps;
            var r = 0;

            for (var i = 0; i < _s.Length; i++)
            {
                if (_s[i] > tol)
                {
                    r++;
                }
            }

            return r;
        }

        #endregion
    }
}