namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;

    sealed class LocalScope : BaseScope
    {
        public LocalScope(IDictionary<String, Object> parent)
            : base(new Dictionary<String, Object>(), parent)
        {
        }
    }
}
