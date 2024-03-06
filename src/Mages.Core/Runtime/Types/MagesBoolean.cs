namespace Mages.Core.Runtime.Types;

using Mages.Core.Runtime.Converters;
using Mages.Core.Runtime.Functions;
using System;
using System.Collections.Generic;

static class MagesBoolean
{
    private static readonly Function Create = new(args =>
    {
        return Curry.MinOne(Create, args) ??
            args[0].ToBoolean();
    });

    public static readonly IDictionary<String, Object> Type = new Dictionary<String, Object>
    {
        { "name", "Boolean" },
        { "create", Create },
    };
}
