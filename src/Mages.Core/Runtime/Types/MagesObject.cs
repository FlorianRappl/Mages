namespace Mages.Core.Runtime.Types;

using Mages.Core.Runtime.Converters;
using Mages.Core.Runtime.Functions;
using System;
using System.Collections.Generic;
using System.Linq;

static class MagesObject
{
    private static readonly Function Create = new(args =>
    {
        return Curry.MinOne(Create, args) ??
            args[0].ToObject();
    });

    public static readonly IDictionary<String, Object> Type = new Dictionary<String, Object>
    {
        { "name", "Object" },
        { "create", Create },
    };

    public static IDictionary<String, Object> GetFullType(IDictionary<String, Object> value)
    {
        var meta = Meta.For(value);
        var info = new Dictionary<String, Object>(Type)
        {
            { "keys", value.Keys.ToArray().ToArrayObject() }
        };

        foreach (var kvp in meta)
        {
            info.Add(kvp.Key, kvp.Value);
        }

        return info;
    }
}
