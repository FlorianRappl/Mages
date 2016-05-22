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
            { "equals", StandardFunctions.Eq },
            { "greaterEquals", StandardFunctions.Geq },
            { "greater", StandardFunctions.Gt },
            { "lessEquals", StandardFunctions.Leq },
            { "less", StandardFunctions.Lt },
            { "modulo", StandardFunctions.Mod },
            { "multiply", StandardFunctions.Mul },
            { "notEquals", StandardFunctions.Neq },
            { "or", StandardFunctions.Or },
            { "divide", StandardFunctions.RDiv },
            { "subtract", StandardFunctions.Sub },
        };
    }
}
