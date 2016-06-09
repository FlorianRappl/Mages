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
    }
}
