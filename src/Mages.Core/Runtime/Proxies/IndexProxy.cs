﻿namespace Mages.Core.Runtime.Proxies;

using System;
using System.Linq;
using System.Reflection;

sealed class IndexProxy : FunctionProxy
{
    public IndexProxy(WrapperObject obj, PropertyInfo[] properties)
        : base(obj, properties.Where(m => m.CanRead).Select(m => m.GetGetMethod()).ToArray())
    {
        _proxy = Helpers.DeclareFunction(Invoke, ["...args"]);
    }

    private Object Invoke(Object[] arguments)
    {
        var parameters = arguments.Select(m => m is not null ? m.GetType() : typeof(Object)).ToArray();
        var method = _methods.Find(parameters, ref arguments);

        if (method is not null)
        {
            return method.Call(_obj, arguments);
        }

        return TryCurry(arguments);
    }
}
