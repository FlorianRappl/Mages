namespace Mages.Repl.Tutorial
{
    using System;
    using System.Collections.Generic;

    sealed class StandardSnippet : ITutorialSnippet
    {
        private readonly Predicate<IDictionary<String, Object>> _checker;

        public StandardSnippet(Predicate<IDictionary<String, Object>> checker)
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
            return _checker.Invoke(scope);
        }
    }
}
