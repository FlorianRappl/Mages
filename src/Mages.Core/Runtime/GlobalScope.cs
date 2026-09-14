namespace Mages.Core.Runtime;

using Mages.Core.Runtime.Functions;
using System;
using System.Collections.Generic;

sealed class GlobalScope(IDictionary<String, Object> scope, AngleUnit angleUnit) : BaseScope(scope ?? new Dictionary<String, Object>(), CreateMapping(angleUnit))
{
    private static IDictionary<String, Object> CreateMapping(AngleUnit angleUnit)
    {
        var mapping = new Dictionary<String, Object>(Global.Mapping);

        if (angleUnit == AngleUnit.Degrees)
        {
            mapping["sin"] = AngleFunctions.Sin;
            mapping["cos"] = AngleFunctions.Cos;
            mapping["tan"] = AngleFunctions.Tan;
            mapping["cot"] = AngleFunctions.Cot;
            mapping["sec"] = AngleFunctions.Sec;
            mapping["csc"] = AngleFunctions.Csc;
            mapping["arcsin"] = AngleFunctions.ArcSin;
            mapping["arccos"] = AngleFunctions.ArcCos;
            mapping["arctan"] = AngleFunctions.ArcTan;
            mapping["arccot"] = AngleFunctions.ArcCot;
            mapping["arcsec"] = AngleFunctions.ArcSec;
            mapping["arccsc"] = AngleFunctions.ArcCsc;
            mapping["arg"] = AngleFunctions.Arg;
        }

        return mapping;
    }

    protected override void SetValue(String key, Object value)
    {
        _scope[key] = value;
    }
}
