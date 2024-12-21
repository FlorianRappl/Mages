namespace Mages.Core.Runtime;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

static class Meta
{
    private static readonly Dictionary<String, Object> _default = [];
    private static readonly ConditionalWeakTable<Object, Dictionary<String, Object>> _mapping = [];

    public static IDictionary<String, Object> For(Object obj)
    {
        if (_mapping.TryGetValue(obj, out var meta))
        {
            return meta;
        }

        return _default;
    }

    public static void Define(Object obj, String name, Object value)
    {
        if (!_mapping.TryGetValue(obj, out var meta))
        {
            meta = [];
            _mapping.Add(obj, meta);
        }

        meta[name] = value;
    }
}