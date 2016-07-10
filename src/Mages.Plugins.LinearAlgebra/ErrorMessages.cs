namespace Mages.Plugins.LinearAlgebra
{
    using System;

    static class ErrorMessages
    {
        public static readonly String SingularSource = "The source matrix is singular.";
        public static readonly String RowMismatch = "The number of rows in the target matrix does not fit the source matrix.";
        public static readonly String DimensionMismatch = "The length of the target matrix does not fit the length of the source matrix.";
        public static readonly String SpdRequired = "The source matrix was not symmetric positive definite.";
    }
}
