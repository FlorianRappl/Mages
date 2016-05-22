namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;

    sealed class VariableScope : BaseScope
    {
        public VariableScope(IDictionary<String, Object> parent)
            : base(new Dictionary<String, Object>(), parent)
        {
        }
    }
}
