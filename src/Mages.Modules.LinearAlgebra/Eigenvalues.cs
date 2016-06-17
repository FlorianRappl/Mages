namespace Mages.Modules.LinearAlgebra
{
    using System;

    /// <summary>
    /// Eigenvalues and eigenvectors of a real matrix. 
    /// If A is symmetric, then A = V * D * V' where the eigenvalue matrix D is
    /// diagonal and the eigenvector matrix V is orthogonal.
    /// I.e. A = V.Multiply(D.Multiply(V.Transpose())) and 
    /// V.Multiply(V.Transpose()) equals the identity matrix.
    /// If A is not symmetric, then the eigenvalue matrix D is block diagonal
    /// with the real eigenvalues in 1-by-1 blocks and any complex eigenvalues,
    /// lambda + i*mu, in 2-by-2 blocks, [lambda, mu; -mu, lambda].  The
    /// columns of V represent the eigenvectors in the sense that A * V = V * D,
    /// i.e. A.Multiply(V) equals V.Multiply(D). The matrix V may be badly
    /// conditioned, or even singular, so the validity of the equation
    /// A = V * D * Inverse(V) depends upon V.cond().
    /// </summary>
    public class Eigenvalues
    {
        #region	 Fields
        
        private readonly Int32 _n;
        private readonly Boolean _symmetric;
        private readonly Double[] _d;
        private readonly Double[] _e;
        private readonly Double[,] _V;
        private readonly Double[,] _H;
        private readonly Double[] _ort;

        #endregion

        #region ctor

        /// <summary>
        /// Check for symmetry, then construct the eigenvalue decomposition
        /// </summary>
        public Eigenvalues(Double[,] matrix)
        {
            _n = matrix.GetLength(1);
            _V = new Double[_n, _n];
            _d = new Double[_n];
            _e = new Double[_n];

            _symmetric = true;

            for (var j = 0; (j < _n) && _symmetric; j++)
            {
                for (var i = 0; (i < _n) && _symmetric; i++)
                {
                    _symmetric = (matrix[i, j] == matrix[j, i]);
                }
            }

            if (_symmetric)
            {
                for (var i = 0; i < _n; i++)
                {
                    for (var j = 0; j < _n; j++)
                    {
                        _V[i, j] = matrix[i, j];
                    }
                }

                // Tridiagonalize.
                tred2();

                // Diagonalize.
                tql2();
            }
            else
            {
                _H = new Double[_n, _n];
                _ort = new Double[_n];

                for (var j = 0; j < _n; j++)
                {
                    for (var i = 0; i < _n; i++)
                    {
                        _H[i, j] = matrix[i, j];
                    }
                }

                // Reduce to Hessenberg form.
                orthes();

                // Reduce Hessenberg to real Schur form.
                hqr2();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the real parts of the eigenvalues.
        /// </summary>
        public Double[] RealEigenvalues
        {
            get { return _d; }
        }

        /// <summary>
        /// Gets the imaginary parts of the eigenvalues.
        /// </summary>
        public Double[] ImagEigenvalues
        {
            get { return _e; }
        }

        /// <summary>
        /// Gets the V matrix.
        /// </summary>
        public Double[,] Eigenvectors
        {
            get { return _V; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Symmetric Householder reduction to tridiagonal form.
        /// </summary>
        private void tred2()
        {
            //  This is derived from the Algol procedures tred2 by
            //  Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            //  Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutine in EISPACK.

            for (var j = 0; j < _n; j++)
            {
                _d[j] = _V[_n - 1, j];
            }

            // Householder reduction to tridiagonal form.

            for (var i = _n - 1; i > 0; i--)
            {
                // Scale to avoid under/overflow.
                var scale = 0.0;
                var h = 0.0;

                for (var k = 0; k < i; k++)
                {
                    scale += Math.Abs(_d[k]);
                }

                if (scale == 0.0)
                {
                    _e[i] = _d[i - 1];

                    for (var j = 0; j < i; j++)
                    {
                        _d[j] = _V[i - 1, j];
                        _V[i, j] = 0.0;
                        _V[j, i] = 0.0;
                    }
                }
                else
                {
                    // Generate Householder vector.
                    for (var k = 0; k < i; k++)
                    {
                        _d[k] /= scale;
                        h += _d[k] * _d[k];
                    }

                    var f = _d[i - 1];
                    var g = Math.Sqrt(h);

                    if (f > 0)
                    {
                        g = -g;
                    }

                    _e[i] = scale * g;
                    h = h - f * g;
                    _d[i - 1] = f - g;

                    for (var j = 0; j < i; j++)
                        _e[j] = 0.0;

                    // Apply similarity transformation to remaining columns.

                    for (var j = 0; j < i; j++)
                    {
                        f = _d[j];
                        _V[j, i] = f;
                        g = _e[j] + _V[j, j] * f;

                        for (var k = j + 1; k <= i - 1; k++)
                        {
                            g += _V[k, j] * _d[k];
                            _e[k] += _V[k, j] * f;
                        }

                        _e[j] = g;
                    }

                    f = 0.0;

                    for (var j = 0; j < i; j++)
                    {
                        _e[j] /= h;
                        f += _e[j] * _d[j];
                    }

                    var hh = f / (h + h);

                    for (var j = 0; j < i; j++)
                    {
                        _e[j] -= hh * _d[j];
                    }

                    for (var j = 0; j < i; j++)
                    {
                        f = _d[j];
                        g = _e[j];

                        for (var k = j; k <= i - 1; k++)
                        {
                            _V[k, j] -= (f * _e[k] + g * _d[k]);
                        }

                        _d[j] = _V[i - 1, j];
                        _V[i, j] = 0.0;
                    }
                }

                _d[i] = h;
            }

            // Accumulate transformations.

            for (var i = 0; i < _n - 1; i++)
            {
                _V[_n - 1, i] = _V[i, i];
                _V[i, i] = 1.0;
                var h = _d[i + 1];

                if (h != 0.0)
                {
                    for (var k = 0; k <= i; k++)
                    {
                        _d[k] = _V[k, i + 1] / h;
                    }

                    for (var j = 0; j <= i; j++)
                    {
                        var g = 0.0;

                        for (var k = 0; k <= i; k++)
                        {
                            g += _V[k, i + 1] * _V[k, j];
                        }

                        for (var k = 0; k <= i; k++)
                        {
                            _V[k, j] -= g * _d[k];
                        }
                    }
                }

                for (var k = 0; k <= i; k++)
                {
                    _V[k, i + 1] = 0.0;
                }
            }

            for (var j = 0; j < _n; j++)
            {
                _d[j] = _V[_n - 1, j];
                _V[_n - 1, j] = 0.0;
            }

            _V[_n - 1, _n - 1] = 1.0;
            _e[0] = 0.0;
        }

        /// <summary>
        /// Symmetric tridiagonal QL algorithm.
        /// </summary>
        private void tql2()
        {
            //  This is derived from the Algol procedures tql2, by
            //  Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            //  Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutine in EISPACK.

            for (var i = 1; i < _n; i++)
            {
                _e[i - 1] = _e[i];
            }

            _e[_n - 1] = 0.0;
            var f = 0.0;
            var tst1 = 0.0;
            var eps = Math.Pow(2.0, -52.0);

            for (var l = 0; l < _n; l++)
            {
                // Find small subdiagonal element

                tst1 = Math.Max(tst1, Math.Abs(_d[l]) + Math.Abs(_e[l]));
                var m = l;

                while (m < _n && Math.Abs(_e[m]) > eps * tst1)
                {
                    m++;
                }

                // If m == l, d[l] is an eigenvalue,
                // otherwise, iterate.

                if (m > l)
                {
                    var iter = 0;

                    do
                    {
                        iter = iter + 1; // (Could check iteration count here.)

                        // Compute implicit shift

                        var g = _d[l];
                        var p = (_d[l + 1] - g) / (2.0 * _e[l]);
                        var r = Helpers.Hypot(p, 1.0);

                        if (p < 0)
                            r = -r;

                        _d[l] = _e[l] / (p + r);
                        _d[l + 1] = _e[l] * (p + r);
                        var dl1 = _d[l + 1];
                        var h = g - _d[l];

                        for (var i = l + 2; i < _n; i++)
                        {
                            _d[i] -= h;
                        }

                        f = f + h;

                        // Implicit QL transformation.

                        p = _d[m];
                        var c = 1.0;
                        var c2 = c;
                        var c3 = c;
                        var el1 = _e[l + 1];
                        var s = 0.0;
                        var s2 = 0.0;

                        for (var i = m - 1; i >= l; i--)
                        {
                            c3 = c2;
                            c2 = c;
                            s2 = s;
                            g = c * _e[i];
                            h = c * p;
                            r = Helpers.Hypot(p, _e[i]);
                            _e[i + 1] = s * r;
                            s = _e[i] / r;
                            c = p / r;
                            p = c * _d[i] - s * g;
                            _d[i + 1] = h + s * (c * g + s * _d[i]);

                            // Accumulate transformation.

                            for (var k = 0; k < _n; k++)
                            {
                                h = _V[k, i + 1];
                                _V[k, i + 1] = s * _V[k, i] + c * h;
                                _V[k, i] = c * _V[k, i] - s * h;
                            }
                        }

                        p = (-s) * s2 * c3 * el1 * _e[l] / dl1;
                        _e[l] = s * p;
                        _d[l] = c * p;

                        // Check for convergence.
                    }
                    while (Math.Abs(_e[l]) > eps * tst1);
                }

                _d[l] = _d[l] + f;
                _e[l] = 0.0;
            }

            // Sort eigenvalues and corresponding vectors.

            for (var i = 0; i < _n - 1; i++)
            {
                var k = i;
                var p = _d[i];

                for (var j = i + 1; j < _n; j++)
                {
                    if (_d[j] < p)
                    {
                        k = j;
                        p = _d[j];
                    }
                }

                if (k != i)
                {
                    _d[k] = _d[i];
                    _d[i] = p;

                    for (var j = 0; j < _n; j++)
                    {
                        p = _V[j, i];
                        _V[j, i] = _V[j, k];
                        _V[j, k] = p;
                    }
                }
            }
        }

        /// <summary>
        /// Nonsymmetric reduction to Hessenberg form.
        /// </summary>
        private void orthes()
        {
            //  This is derived from the Algol procedures orthes and ortran,
            //  by Martin and Wilkinson, Handbook for Auto. Comp.,
            //  Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutines in EISPACK.

            var low = 0;
            var high = _n - 1;

            for (var m = low + 1; m <= high - 1; m++)
            {
                // Scale column.
                var scale = 0.0;

                for (var i = m; i <= high; i++)
                {
                    scale = scale + Math.Abs(_H[i, m - 1]);
                }

                if (scale != 0.0)
                {
                    // Compute Householder transformation.
                    var h = 0.0;

                    for (var i = high; i >= m; i--)
                    {
                        _ort[i] = _H[i, m - 1] / scale;
                        h += _ort[i] * _ort[i];
                    }

                    var g = Math.Sqrt(h);

                    if (_ort[m] > 0)
                    {
                        g = -g;
                    }

                    h = h - _ort[m] * g;
                    _ort[m] = _ort[m] - g;

                    // Apply Householder similarity transformation
                    // H = (I-u*u'/h)*H*(I-u*u')/h)

                    for (var j = m; j < _n; j++)
                    {
                        var f = 0.0;

                        for (var i = high; i >= m; i--)
                        {
                            f += _ort[i] * _H[i, j];
                        }

                        f = f / h;

                        for (var i = m; i <= high; i++)
                        {
                            _H[i, j] -= f * _ort[i];
                        }
                    }

                    for (var i = 0; i <= high; i++)
                    {
                        var f = 0.0;

                        for (var j = high; j >= m; j--)
                        {
                            f += _ort[j] * _H[i, j];
                        }

                        f = f / h;

                        for (var j = m; j <= high; j++)
                        {
                            _H[i, j] -= f * _ort[j];
                        }
                    }

                    _ort[m] = scale * _ort[m];
                    _H[m, m - 1] = scale * g;
                }
            }

            // Accumulate transformations (Algol's ortran).

            for (var i = 0; i < _n; i++)
            {
                for (var j = 0; j < _n; j++)
                {
                    _V[i, j] = (i == j ? 1.0 : 0.0);
                }
            }

            for (var m = high - 1; m >= low + 1; m--)
            {
                if (_H[m, m - 1] != 0.0)
                {
                    for (var i = m + 1; i <= high; i++)
                    {
                        _ort[i] = _H[i, m - 1];
                    }

                    for (var j = m; j <= high; j++)
                    {
                        var g = 0.0;

                        for (var i = m; i <= high; i++)
                        {
                            g += _ort[i] * _V[i, j];
                        }

                        // Double division avoids possible underflow
                        g = (g / _ort[m]) / _H[m, m - 1];

                        for (var i = m; i <= high; i++)
                        {
                            _V[i, j] += g * _ort[i];
                        }
                    }
                }
            }
        }
        
        private Complex cdiv(Double xr, Double xi, Double yr, Double yi)
        {
            var r = 0.0;
            var d = 0.0;

            if (Math.Abs(yr) > Math.Abs(yi))
            {
                r = yi / yr;
                d = yr + r * yi;
                return new Complex
                {
                    Real = (xr + r * xi) / d,
                    Imag = (xi - r * xr) / d
                };
            }
            else
            {
                r = yr / yi;
                d = yi + r * yr;
                return new Complex
                {
                    Real = (r * xr + xi) / d,
                    Imag = (r * xi - xr) / d
                };
            }
        }

        /// <summary>
        /// Nonsymmetric reduction from Hessenberg to real Schur form.
        /// </summary>
        private void hqr2()
        {
            //  This is derived from the Algol procedure hqr2,
            //  by Martin and Wilkinson, Handbook for Auto. Comp.,
            //  Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutine in EISPACK.

            // Initialize
            var nn = _n;
            var n = nn - 1;
            var low = 0;
            var high = nn - 1;
            var eps = Math.Pow(2.0, -52.0);
            var exshift = 0.0;
            var p = 0.0;
            var q = 0.0;
            var r = 0.0;
            var s = 0.0;
            var z = 0.0;
            var t = 0.0;
            var w = 0.0;
            var x = 0.0;
            var y = 0.0;

            // Store roots isolated by balanc and compute matrix norm

            var norm = 0.0;

            for (var i = 0; i < nn; i++)
            {
                if (i < low | i > high)
                {
                    _d[i] = _H[i, i];
                    _e[i] = 0.0;
                }

                for (var j = Math.Max(i - 1, 0); j < nn; j++)
                {
                    norm += Math.Abs(_H[i, j]);
                }
            }

            // Outer loop over eigenvalue index

            var iter = 0;

            while (n >= low)
            {
                // Look for single small sub-diagonal element

                var l = n;

                while (l > low)
                {
                    s = Math.Abs(_H[l - 1, l - 1]) + Math.Abs(_H[l, l]);

                    if (s == 0.0)
                        s = norm;

                    if (Math.Abs(_H[l, l - 1]) < eps * s)
                        break;

                    l--;
                }

                // Check for convergence
                // One root found

                if (l == n)
                {
                    _H[n, n] = _H[n, n] + exshift;
                    _d[n] = _H[n, n];
                    _e[n] = 0.0;
                    n--;
                    iter = 0;

                    // Two roots found
                }
                else if (l == n - 1)
                {
                    w = _H[n, n - 1] * _H[n - 1, n];
                    p = (_H[n - 1, n - 1] - _H[n, n]) / 2.0;
                    q = p * p + w;
                    z = Math.Sqrt(Math.Abs(q));
                    _H[n, n] = _H[n, n] + exshift;
                    _H[n - 1, n - 1] = _H[n - 1, n - 1] + exshift;
                    x = _H[n, n];

                    // Real pair

                    if (q >= 0)
                    {
                        z = p >= 0 ? p + z : p - z;
                        _d[n - 1] = x + z;
                        _d[n] = _d[n - 1];

                        if (z != 0.0)
                            _d[n] = x - w / z;

                        _e[n - 1] = 0.0;
                        _e[n] = 0.0;
                        x = _H[n, n - 1];
                        s = Math.Abs(x) + Math.Abs(z);
                        p = x / s;
                        q = z / s;
                        r = Math.Sqrt(p * p + q * q);
                        p = p / r;
                        q = q / r;

                        // Row modification

                        for (var j = n - 1; j < nn; j++)
                        {
                            z = _H[n - 1, j];
                            _H[n - 1, j] = q * z + p * _H[n, j];
                            _H[n, j] = q * _H[n, j] - p * z;
                        }

                        // Column modification

                        for (var i = 0; i <= n; i++)
                        {
                            z = _H[i, n - 1];
                            _H[i, n - 1] = q * z + p * _H[i, n];
                            _H[i, n] = q * _H[i, n] - p * z;
                        }

                        // Accumulate transformations

                        for (var i = low; i <= high; i++)
                        {
                            z = _V[i, n - 1];
                            _V[i, n - 1] = q * z + p * _V[i, n];
                            _V[i, n] = q * _V[i, n] - p * z;
                        }

                        // Complex pair
                    }
                    else
                    {
                        _d[n - 1] = x + p;
                        _d[n] = x + p;
                        _e[n - 1] = z;
                        _e[n] = -z;
                    }

                    n = n - 2;
                    iter = 0;

                    // No convergence yet
                }
                else
                {
                    // Form shift

                    x = _H[n, n];
                    y = 0.0;
                    w = 0.0;

                    if (l < n)
                    {
                        y = _H[n - 1, n - 1];
                        w = _H[n, n - 1] * _H[n - 1, n];
                    }

                    // Wilkinson's original ad hoc shift

                    if (iter == 10)
                    {
                        exshift += x;

                        for (var i = low; i <= n; i++)
                        {
                            _H[i, i] -= x;
                        }

                        s = Math.Abs(_H[n, n - 1]) + Math.Abs(_H[n - 1, n - 2]);
                        x = y = 0.75 * s;
                        w = (-0.4375) * s * s;
                    }

                    // MATLAB's new ad hoc shift

                    if (iter == 30)
                    {
                        s = (y - x) / 2.0;
                        s = s * s + w;

                        if (s > 0)
                        {
                            s = Math.Sqrt(s);

                            if (y < x)
                                s = -s;

                            s = x - w / ((y - x) / 2.0 + s);

                            for (var i = low; i <= n; i++)
                            {
                                _H[i, i] -= s;
                            }

                            exshift += s;
                            x = y = w = 0.964;
                        }
                    }

                    iter = iter + 1; // (Could check iteration count here.)

                    // Look for two consecutive small sub-diagonal elements

                    var m = n - 2;

                    while (m >= l)
                    {
                        z = _H[m, m];
                        r = x - z;
                        s = y - z;
                        p = (r * s - w) / _H[m + 1, m] + _H[m, m + 1];
                        q = _H[m + 1, m + 1] - z - r - s;
                        r = _H[m + 2, m + 1];
                        s = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);
                        p = p / s;
                        q = q / s;
                        r = r / s;

                        if (m == l)
                            break;

                        if (Math.Abs(_H[m, m - 1]) * (Math.Abs(q) + Math.Abs(r)) < eps * (Math.Abs(p) * (Math.Abs(_H[m - 1, m - 1]) + Math.Abs(z) + Math.Abs(_H[m + 1, m + 1]))))
                            break;

                        m--;
                    }

                    for (var i = m + 2; i <= n; i++)
                    {
                        _H[i, i - 2] = 0.0;

                        if (i > m + 2)
                        {
                            _H[i, i - 3] = 0.0;
                        }
                    }

                    // Double QR step involving rows l:n and columns m:n

                    for (var k = m; k <= n - 1; k++)
                    {
                        var notlast = (k != n - 1);

                        if (k != m)
                        {
                            p = _H[k, k - 1];
                            q = _H[k + 1, k - 1];
                            r = (notlast ? _H[k + 2, k - 1] : 0.0);
                            x = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);

                            if (x != 0.0)
                            {
                                p = p / x;
                                q = q / x;
                                r = r / x;
                            }
                        }

                        if (x == 0.0)
                            break;

                        s = Math.Sqrt(p * p + q * q + r * r);

                        if (p < 0)
                            s = -s;

                        if (s != 0)
                        {
                            if (k != m)
                                _H[k, k - 1] = (-s) * x;
                            else if (l != m)
                                _H[k, k - 1] = -_H[k, k - 1];

                            p = p + s;
                            x = p / s;
                            y = q / s;
                            z = r / s;
                            q = q / p;
                            r = r / p;

                            // Row modification

                            for (var j = k; j < nn; j++)
                            {
                                p = _H[k, j] + q * _H[k + 1, j];

                                if (notlast)
                                {
                                    p = p + r * _H[k + 2, j];
                                    _H[k + 2, j] = _H[k + 2, j] - p * z;
                                }

                                _H[k, j] = _H[k, j] - p * x;
                                _H[k + 1, j] = _H[k + 1, j] - p * y;
                            }

                            // Column modification

                            for (var i = 0; i <= Math.Min(n, k + 3); i++)
                            {
                                p = x * _H[i, k] + y * _H[i, k + 1];

                                if (notlast)
                                {
                                    p = p + z * _H[i, k + 2];
                                    _H[i, k + 2] = _H[i, k + 2] - p * r;
                                }

                                _H[i, k] = _H[i, k] - p;
                                _H[i, k + 1] = _H[i, k + 1] - p * q;
                            }

                            // Accumulate transformations

                            for (var i = low; i <= high; i++)
                            {
                                p = x * _V[i, k] + y * _V[i, k + 1];

                                if (notlast)
                                {
                                    p = p + z * _V[i, k + 2];
                                    _V[i, k + 2] = _V[i, k + 2] - p * r;
                                }

                                _V[i, k] = _V[i, k] - p;
                                _V[i, k + 1] = _V[i, k + 1] - p * q;
                            }
                        } // (s != 0)
                    } // k loop
                } // check convergence
            } // while (n >= low)

            // Backsubstitute to find vectors of upper triangular form

            if (norm == 0.0)
                return;

            for (n = nn - 1; n >= 0; n--)
            {
                p = _d[n];
                q = _e[n];

                // Real vector

                if (q == 0)
                {
                    var l = n;
                    _H[n, n] = 1.0;

                    for (var i = n - 1; i >= 0; i--)
                    {
                        w = _H[i, i] - p;
                        r = 0.0;

                        for (var j = l; j <= n; j++)
                        {
                            r += _H[i, j] * _H[j, n];
                        }

                        if (_e[i] < 0.0)
                        {
                            z = w;
                            s = r;
                        }
                        else
                        {
                            l = i;

                            if (_e[i] == 0.0)
                            {
                                if (w != 0.0)
                                    _H[i, n] = (-r) / w;
                                else
                                    _H[i, n] = (-r) / (eps * norm);

                                // Solve real equations
                            }
                            else
                            {
                                x = _H[i, i + 1];
                                y = _H[i + 1, i];
                                q = (_d[i] - p) * (_d[i] - p) + _e[i] * _e[i];
                                t = (x * s - z * r) / q;
                                _H[i, n] = t;

                                if (Math.Abs(x) > Math.Abs(z))
                                    _H[i + 1, n] = (-r - w * t) / x;
                                else
                                    _H[i + 1, n] = (-s - y * t) / z;
                            }

                            // Overflow control

                            t = Math.Abs(_H[i, n]);

                            if ((eps * t) * t > 1)
                            {
                                for (var j = i; j <= n; j++)
                                {
                                    _H[j, n] = _H[j, n] / t;
                                }
                            }
                        }
                    }

                    // Complex vector
                }
                else if (q < 0)
                {
                    var l = n - 1;

                    // Last vector component imaginary so matrix is triangular

                    if (Math.Abs(_H[n, n - 1]) > Math.Abs(_H[n - 1, n]))
                    {
                        _H[n - 1, n - 1] = q / _H[n, n - 1];
                        _H[n - 1, n] = (-(_H[n, n] - p)) / _H[n, n - 1];
                    }
                    else
                    {
                        var g = cdiv(0.0, -_H[n - 1, n], _H[n - 1, n - 1] - p, q);
                        _H[n - 1, n - 1] = g.Real;
                        _H[n - 1, n] = g.Imag;
                    }

                    _H[n, n - 1] = 0.0;
                    _H[n, n] = 1.0;

                    for (var i = n - 2; i >= 0; i--)
                    {
                        var ra = 0.0;
                        var sa = 0.0;
                        var vr = 0.0;
                        var vi = 0.0;

                        for (var j = l; j <= n; j++)
                        {
                            ra += _H[i, j] * _H[j, n - 1];
                            sa += _H[i, j] * _H[j, n];
                        }

                        w = _H[i, i] - p;

                        if (_e[i] < 0.0)
                        {
                            z = w;
                            r = ra;
                            s = sa;
                        }
                        else
                        {
                            l = i;

                            if (_e[i] == 0)
                            {
                                var g = cdiv(-ra, -sa, w, q);
                                _H[i, n - 1] = g.Real;
                                _H[i, n] = g.Imag;
                            }
                            else
                            {
                                // Solve complex equations
                                x = _H[i, i + 1];
                                y = _H[i + 1, i];
                                vr = (_d[i] - p) * (_d[i] - p) + _e[i] * _e[i] - q * q;
                                vi = (_d[i] - p) * 2.0 * q;

                                if (vr == 0.0 & vi == 0.0)
                                {
                                    vr = eps * norm * (Math.Abs(w) + Math.Abs(q) + Math.Abs(x) + Math.Abs(y) + Math.Abs(z));
                                }

                                var g = cdiv(x * r - z * ra + q * sa, x * s - z * sa - q * ra, vr, vi);
                                _H[i, n - 1] = g.Real;
                                _H[i, n] = g.Imag;

                                if (Math.Abs(x) > (Math.Abs(z) + Math.Abs(q)))
                                {
                                    _H[i + 1, n - 1] = (-ra - w * _H[i, n - 1] + q * _H[i, n]) / x;
                                    _H[i + 1, n] = (-sa - w * _H[i, n] - q * _H[i, n - 1]) / x;
                                }
                                else
                                {
                                    g = cdiv(-r - y * _H[i, n - 1], -s - y * _H[i, n], z, q);
                                    _H[i + 1, n - 1] = g.Real;
                                    _H[i + 1, n] = g.Imag;
                                }
                            }

                            // Overflow control

                            t = Math.Max(Math.Abs(_H[i, n - 1]), Math.Abs(_H[i, n]));

                            if ((eps * t) * t > 1)
                            {
                                for (var j = i; j <= n; j++)
                                {
                                    _H[j, n - 1] = _H[j, n - 1] / t;
                                    _H[j, n] = _H[j, n] / t;
                                }
                            }
                        }
                    }
                }
            }

            // Vectors of isolated roots

            for (var i = 0; i < nn; i++)
            {
                if (i < low | i > high)
                {
                    for (var j = i; j < nn; j++)
                    {
                        _V[i, j] = _H[i, j];
                    }
                }
            }

            // Back transformation to get eigenvectors of original matrix

            for (var j = nn - 1; j >= low; j--)
            {
                for (var i = low; i <= high; i++)
                {
                    z = 0.0;

                    for (var k = low; k <= Math.Min(j, high); k++)
                    {
                        z = z + _V[i, k] * _H[k, j];
                    }

                    _V[i,j] = z;
                }
            }
        }

        #endregion

        #region Complex

        struct Complex
        {
            public Double Real;
            public Double Imag;
        }

        #endregion
    }
}