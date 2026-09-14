namespace Mages.Core.Runtime.Functions;

using Mages.Core.Runtime.Converters;
using System;
using System.Numerics;

static class AngleFunctions
{
    private const Double DegreesToRadians = Math.PI / 180.0;
    private const Double RadiansToDegrees = 180.0 / Math.PI;

    public static readonly Function Sin = Create(StandardFunctions.Sin, true);
    public static readonly Function Cos = Create(StandardFunctions.Cos, true);
    public static readonly Function Tan = Create(StandardFunctions.Tan, true);
    public static readonly Function Cot = Create(StandardFunctions.Cot, true);
    public static readonly Function Sec = Create(StandardFunctions.Sec, true);
    public static readonly Function Csc = Create(StandardFunctions.Csc, true);
    public static readonly Function ArcSin = Create(StandardFunctions.ArcSin, false);
    public static readonly Function ArcCos = Create(StandardFunctions.ArcCos, false);
    public static readonly Function ArcTan = Create(StandardFunctions.ArcTan, false);
    public static readonly Function ArcCot = Create(StandardFunctions.ArcCot, false);
    public static readonly Function ArcSec = Create(StandardFunctions.ArcSec, false);
    public static readonly Function ArcCsc = Create(StandardFunctions.ArcCsc, false);
    public static readonly Function Arg = Create(ComplexFunctions.Arg, false);

    private static Function Create(Function function, Boolean convertInput)
    {
        Function result = null;
        result = Helpers.DeclareFunction(args =>
        {
            return Curry.MinOne(result, args) ??
                (convertInput ? function.Invoke(ConvertInput(args)) : ConvertOutput(function.Invoke(args)));
        }, ["x"]);
        return result;
    }

    private static Object[] ConvertInput(Object[] args)
    {
        var result = (Object[])args.Clone();
        result[0] = ConvertValue(result[0], DegreesToRadians);
        return result;
    }

    private static Object ConvertOutput(Object value) => ConvertValue(value, RadiansToDegrees);

    private static Object ConvertValue(Object value, Double factor)
    {
        return value switch
        {
            Double number => number * factor,
            Complex number => number * factor,
            Double[,] matrix => matrix.ForEach(number => number * factor),
            Complex[,] matrix => matrix.ForEach(number => number * factor),
            _ => value,
        };
    }
}