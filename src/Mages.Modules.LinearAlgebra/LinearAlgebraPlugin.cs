namespace Mages.Modules.LinearAlgebra
{
    using Decompositions;
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
                    case 0:
                        return 0.0;
                    case 1:
                        return matrix[0, 0];
                    case 2:
                        return Helpers.Det2(matrix);
                    case 3:
                        return Helpers.Det3(matrix);
                    case 4:
                        return Helpers.Det4(matrix);
                    default:
                        return LUDecomposition.Determinant(matrix);
                }
            }

            return 0.0;
        }
    }
}
