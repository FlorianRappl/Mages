﻿namespace Mages.Core.Runtime.Types;

using Mages.Core.Runtime.Functions;
using System;
using System.Collections.Generic;

static class MagesFunction
{
    private static readonly Function Create = new(args =>
    {
        return Curry.MinOne(Create, args) ??
            (args[0] as Function);
    });

    public static readonly IDictionary<String, Object> Type = new Dictionary<String, Object>
    {
        { "name", "Function" },
        { "create", Create },
    };
}
