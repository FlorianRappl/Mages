namespace Mages.Plugins.LinearAlgebra
{
    using Mages.Plugins.LinearAlgebra.Decompositions;
    using Mages.Plugins.LinearAlgebra.Solvers;
    using System;

    public static class LinearAlgebraPlugin
    {
        public static readonly String Name = "Linear Algebra";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";

        public static Object Lu(Double[,] matrix)
        {
            var lu = new LUDecomposition(matrix);
            return Helpers.CreateObject(
                "l", lu.L, 
                "u", lu.U, 
                "pivot", lu.Pivot, 
                "singular", !lu.IsNonSingular
            );
        }

        public static Object Qr(Double[,] matrix)
        {
            var qr = QRDecomposition.Create(matrix);
            return Helpers.CreateObject(
                "q", qr.Q,
                "r", qr.R,
                "full", qr.HasFullRank
            );
        }

        public static Object Cholesky(Double[,] matrix)
        {
            var chol = new CholeskyDecomposition(matrix);
            return Helpers.CreateObject(
                "l", chol.L,
                "spd", chol.IsSpd
            );
        }

        public static Object Householder(Double[,] matrix)
        {
            var qr = new HouseholderDecomposition(matrix);
            return Helpers.CreateObject(
                "q", qr.Q,
                "r", qr.R,
                "h", qr.H,
                "full", qr.HasFullRank
            );
        }

        public static Object Givens(Double[,] matrix)
        {
            var qr = new GivensDecomposition(matrix);
            return Helpers.CreateObject(
                "q", qr.Q,
                "r", qr.R,
                "full", qr.HasFullRank
            );
        }

        public static Double[,] Inverse(Double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var target = Helpers.One(cols);

            if (cols < 24)
            {
                var lu = new LUDecomposition(matrix);
                return lu.Solve(target);
            }
            else if (Helpers.IsSymmetric(matrix))
            {
                var cho = new CholeskyDecomposition(matrix);
                return cho.Solve(target);
            }
            else
            {
                var qr = QRDecomposition.Create(matrix);
                return qr.Solve(target);
            }
        }

        public static Object Svd(Double[,] matrix)
        {
            var svd = new SingularValueDecomposition(matrix);
            return Helpers.CreateObject(
                "condition", svd.Condition,
                "s", svd.S,
                "v", svd.V,
                "u", svd.U,
                "singular", svd.SingularValues
            );
        }

        public static Double Trace(Double[,] matrix)
        {
            var length = Math.Min(matrix.GetLength(0), matrix.GetLength(1));
            var sum = 0.0;

            for (var i = 0; i < length; i++)
            {
                sum += matrix[i, i];
            }

            return sum;
        }

        public static Double Det(Double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            if (rows == columns)
            {
                switch (rows)
                {
                    case 0: return 0.0;
                    case 1: return matrix[0, 0];
                    case 2: return Helpers.Det2(matrix);
                    case 3: return Helpers.Det3(matrix);
                    case 4: return Helpers.Det4(matrix);
                }

                return LUDecomposition.Determinant(matrix);
            }

            return 0.0;
        }

        public static Double[,] Cg(Double[,] A, Double[,] b)
        {
            var cg = new CGSolver(A);
            return cg.Solve(b);
        }

        public static Double[,] Gmres(Double[,] A, Double[,] b)
        {
            var cg = new GMRESkSolver(A);
            return cg.Solve(b);
        }

        public static Object Eigen(Double[,] A)
        {
            var ev = new Eigenvalues(A);
            return Helpers.CreateObject(
                "real", Helpers.ToMatrix(ev.RealEigenvalues),
                "imag", Helpers.ToMatrix(ev.ImagEigenvalues),
                "vec", ev.Eigenvectors
            );
        }

        public static Double[,] Solve(Double[,] A, Double[,] b)
        {
            if (Helpers.IsSymmetric(A))
            {
                return Cg(A, b);
            }
            else if (A.GetLength(0) == A.GetLength(1) && A.GetLength(0) > 64) // Is there a way to "guess" a good number for this?
            {
                var gmres = new GMRESkSolver(A);
                gmres.Restart = 30;
                return gmres.Solve(b);
            }

            return Helpers.Multiply(Inverse(A), b);
        }
    }
}
