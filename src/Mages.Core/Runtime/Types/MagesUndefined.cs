namespace Mages.Core.Runtime.Types;

using Mages.Core.Runtime.Functions;
using System;
using System.Collections.Generic;

static class MagesUndefined
{
    private static readonly Function Create = new Function(args =>
    {
        return Curry.MinOne(Create, args) ?? null;
    });

    public static readonly IDictionary<String, Object> Type = new Dictionary<String, Object>
    {
        { "name", "Undefined" },
        { "create", Create },
    };
}
