namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Collections.Generic;

    static class Global
    {
        public static readonly IDictionary<String, Object> Mapping = new Dictionary<String, Object>
        {
            { "abs", StandardFunctions.Abs },
            { "ceil", StandardFunctions.Ceil },
            { "exp", StandardFunctions.Exp },
            { "factorial", StandardFunctions.Factorial },
            { "floor", StandardFunctions.Floor },
            { "round", StandardFunctions.Round },
            { "log", StandardFunctions.Log },
            { "negative", StandardFunctions.Negative },
            { "not", StandardFunctions.Not },
            { "positive", StandardFunctions.Positive },
            { "pow", StandardFunctions.Pow },
            { "sign", StandardFunctions.Sign },
            { "sqrt", StandardFunctions.Sqrt },
            { "transpose", StandardFunctions.Transpose },
            { "add", StandardFunctions.Add },
            { "and", StandardFunctions.And },
            { "equals", ComparisonFunctions.Eq },
            { "notEquals", ComparisonFunctions.Neq },
            { "greaterEquals", ComparisonFunctions.Geq },
            { "greater", ComparisonFunctions.Gt },
            { "lessEquals", ComparisonFunctions.Leq },
            { "less", ComparisonFunctions.Lt },
            { "modulo", StandardFunctions.Mod },
            { "multiply", StandardFunctions.Mul },
            { "or", StandardFunctions.Or },
            { "divide", StandardFunctions.RDiv },
            { "subtract", StandardFunctions.Sub },
            { "rand", StandardFunctions.Rand },
            { "sin", TrigonometricFunctions.Sin },
            { "cos", TrigonometricFunctions.Cos },
            { "tan", TrigonometricFunctions.Tan },
            { "sinh", TrigonometricFunctions.Sinh },
            { "cosh", TrigonometricFunctions.Cosh },
            { "tanh", TrigonometricFunctions.Tanh },
            { "arcsin", TrigonometricFunctions.ArcSin },
            { "arccos", TrigonometricFunctions.ArcCos },
            { "arctan", TrigonometricFunctions.ArcTan },
            { "isnan", LogicalFunctions.IsNaN },
            { "isint", LogicalFunctions.IsInt },
            { "isprime", LogicalFunctions.IsPrime },
            { "isinfty", LogicalFunctions.IsInfty },
            { "min", ComparisonFunctions.Min },
            { "max", ComparisonFunctions.Max },
            { "sort", ComparisonFunctions.Sort },
            { "reverse", ComparisonFunctions.Reverse }
        };
    }
}
