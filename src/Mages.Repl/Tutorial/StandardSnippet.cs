namespace Mages.Repl.Tutorial
{
    using System;
    using System.Collections.Generic;

    sealed class StandardSnippet<T> : ITutorialSnippet
    {
        private readonly Func<T, IDictionary<String, Object>, Boolean> _checker;

        public StandardSnippet(Func<T, IDictionary<String, Object>, Boolean> checker)
        {
            _checker = checker;
        }

        public String Description
        {
            get;
            set;
        }

        public String ExampleCommand
        {
            get;
            set;
        }

        public IEnumerable<String> Hints
        {
            get;
            set;
        }

        public String Solution
        {
            get;
            set;
        }

        public String Task
        {
            get;
            set;
        }

        public String Title
        {
            get;
            set;
        }

        public Boolean Check(IDictionary<String, Object> scope)
        {
            var value = scope["ans"];
            return value is T ? _checker.Invoke((T)value, scope) : false;
        }
    }
}
