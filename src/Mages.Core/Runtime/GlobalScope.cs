namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    sealed class GlobalScope : Scope
    {
        void LoadType(Type type)
        {
            foreach (var f in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                _scope.Add(f.Name.ToLower(), f.GetValue(null));
        }

        public GlobalScope(IDictionary<String, Object> scope): base(null)
        {
            if (scope == null)
            {
                LoadType(typeof(StandardFunctions));
                LoadType(typeof(StandardOperators));
                return;
            }

            foreach (var item in scope)
            {
                _scope.Add(item);
            }
        }

        public override object this[string key]
        {
            get
            {
                return _scope[key];
            }
            set
            {
                _scope[key] = value;
            }
        }

        public override bool TryGetValue(string key, out object value)
        {
            return _scope.TryGetValue(key, out value);
        }
    }
}
