﻿namespace Mages.Core.Runtime;

using Mages.Core.Runtime.Functions;
using System;
using System.Collections.Generic;

static class Global
{
    public static readonly IDictionary<String, Object> Mapping = new Dictionary<String, Object>
    {
        // Functions
        { "abs", StandardFunctions.Abs },
        { "not", StandardFunctions.Not },
        { "type", StandardFunctions.Type },
        { "factorial", StandardFunctions.Factorial },
        { "positive", StandardFunctions.Positive },
        { "negative", StandardFunctions.Negative },
        { "transpose", StandardFunctions.Transpose },
        { "pow", StandardFunctions.Pow },
        { "add", StandardFunctions.Add },
        { "and", StandardFunctions.And },
        { "eq", StandardFunctions.Eq },
        { "neq", StandardFunctions.Neq },
        { "leq", StandardFunctions.Leq },
        { "lt", StandardFunctions.Lt },
        { "geq", StandardFunctions.Geq },
        { "gt", StandardFunctions.Gt },
        { "mod", StandardFunctions.Mod },
        { "mul", StandardFunctions.Mul },
        { "or", StandardFunctions.Or },
        { "div", StandardFunctions.RDiv },
        { "sub", StandardFunctions.Sub },
        { "ceil", StandardFunctions.Ceil },
        { "exp", StandardFunctions.Exp },
        { "floor", StandardFunctions.Floor },
        { "round", StandardFunctions.Round },
        { "log2", StandardFunctions.Log2 },
        { "log10", StandardFunctions.Log10 },
        { "log", StandardFunctions.Log },
        { "sign", StandardFunctions.Sign },
        { "gamma", StandardFunctions.Gamma },
        { "sqrt", StandardFunctions.Sqrt },
        { "rand", StandardFunctions.Rand },
        { "randi", StandardFunctions.Randi },
        { "sin", StandardFunctions.Sin },
        { "cos", StandardFunctions.Cos },
        { "tan", StandardFunctions.Tan },
        { "cot", StandardFunctions.Cot },
        { "sec", StandardFunctions.Sec },
        { "csc", StandardFunctions.Csc },
        { "sinh", StandardFunctions.Sinh },
        { "cosh", StandardFunctions.Cosh },
        { "tanh", StandardFunctions.Tanh },
        { "coth", StandardFunctions.Coth },
        { "sech", StandardFunctions.Sech },
        { "csch", StandardFunctions.Csch },
        { "arcsin", StandardFunctions.ArcSin },
        { "arccos", StandardFunctions.ArcCos },
        { "arctan", StandardFunctions.ArcTan },
        { "arccot", StandardFunctions.ArcCot },
        { "arcsec", StandardFunctions.ArcSec },
        { "arccsc", StandardFunctions.ArcCsc },
        { "arsinh", StandardFunctions.ArSinh },
        { "arcosh", StandardFunctions.ArCosh },
        { "artanh", StandardFunctions.ArTanh },
        { "arcoth", StandardFunctions.ArCoth },
        { "arsech", StandardFunctions.ArSech },
        { "arcsch", StandardFunctions.ArCsch },
        { "isnan", StandardFunctions.IsNaN },
        { "isint", StandardFunctions.IsInt },
        { "isprime", StandardFunctions.IsPrime },
        { "isinfty", StandardFunctions.IsInfty },
        { "min", StandardFunctions.Min },
        { "max", StandardFunctions.Max },
        { "sort", StandardFunctions.Sort },
        { "reverse", StandardFunctions.Reverse },
        { "throw", StandardFunctions.Throw },
        { "catch", StandardFunctions.Catch },
        { "length", StandardFunctions.Length },
        { "keys", StandardFunctions.Keys },
        { "sum", StandardFunctions.Sum },
        { "any", StandardFunctions.Any },
        { "all", StandardFunctions.All },
        { "stringify", Stringify.Default },
        { "json", Stringify.Json },
        { "list", StandardFunctions.List },
        { "is", StandardFunctions.Is },
        { "as", StandardFunctions.As },
        { "map", StandardFunctions.Map },
        { "reduce", StandardFunctions.Reduce },
        { "where", StandardFunctions.Where },
        { "concat", StandardFunctions.Concat },
        { "zip", StandardFunctions.Zip },
        { "intersect", StandardFunctions.Intersection },
        { "union", StandardFunctions.Union },
        { "except", StandardFunctions.Except },
        { "hasKey", StandardFunctions.HasKey },
        { "getValue", StandardFunctions.GetValue },
        { "shuffle", StandardFunctions.Shuffle },
        { "clip", StandardFunctions.Clip },
        { "regex", StandardFunctions.Regex },
        { "lerp", StandardFunctions.Lerp },
        { "clamp", StandardFunctions.Clamp },
        { "cmplx", ComplexFunctions.Cmplx },
        { "conj", ComplexFunctions.Conj },
        { "real", ComplexFunctions.Real },
        { "imag", ComplexFunctions.Imag },
        { "arg", ComplexFunctions.Arg },
        { "jsx", StandardFunctions.Jsx },
        { "html", Stringify.Html },

        // Constants
        { "e", Constants.E },
        { "i", Constants.I },
        { "j", Constants.J },
        { "I", Constants.I },
        { "J", Constants.J },
        { "exp0", 1.0 },
        { "exp1", Constants.E },
        { "alpha", Constants.Alpha },
        { "delta", Constants.Delta },
        { "gamma1", Constants.Gamma1},
        { "gauss", Constants.Gauss },
        { "omega", Constants.Omega },
        { "catalan", Constants.Catalan },
        { "phi", Constants.Phi },
        { "deg", Constants.Deg },
    };
}
