namespace Mages.Core;

using System;

static class Undefined
{
    public static readonly Object Instance = new();

    public static Boolean IsUndefined(this Object o)
    {
        return Object.ReferenceEquals(o, Instance);
    }
}
