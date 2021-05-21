namespace Mages.Core.Runtime.Types
{
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Collections.Generic;

    static class MagesString
    {
        private static readonly Function Create = new Function(args =>
        {
            return Curry.MinOne(Create, args) ??
                Stringify.This(args[0]);
        });

        public static readonly IDictionary<String, Object> Type = new Dictionary<String, Object>
        {
            { "name", "String" },
            { "create", Create },
        };
    }
}
