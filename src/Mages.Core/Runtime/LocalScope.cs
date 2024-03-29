﻿namespace Mages.Core.Runtime;

using System;
using System.Collections.Generic;

sealed class LocalScope(IDictionary<String, Object> parent) : BaseScope(new Dictionary<String, Object>(), parent)
{
    protected override void SetValue(String key, Object value)
    {
        if (_scope.ContainsKey(key))
        {
            _scope[key] = value;
        }
        else
        {
            _parent[key] = value;
        }
    }
}
