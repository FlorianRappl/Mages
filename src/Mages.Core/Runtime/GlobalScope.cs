namespace Mages.Core.Runtime;

using System;
using System.Collections.Generic;

sealed class GlobalScope(IDictionary<String, Object> scope) : BaseScope(scope ?? new Dictionary<String, Object>(), new Dictionary<String, Object>(Global.Mapping))
{
    protected override void SetValue(String key, Object value)
    {
        _scope[key] = value;
    }
}
