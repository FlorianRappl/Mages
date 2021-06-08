namespace Mages.Core.Runtime.Types
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Collections.Generic;

    static class MagesComplex
    {
        private static readonly Function Create = new Function(args =>
        {
            return Curry.MinOne(Create, args) ??
                args[0].ToComplex();
        });

        public static readonly IDictionary<String, Object> Type = new Dictionary<String, Object>
        {
            { "name", "Complex" },
            { "create", Create },
        };
    }
}
