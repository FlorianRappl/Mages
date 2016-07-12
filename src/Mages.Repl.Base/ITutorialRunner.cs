namespace Mages.Repl
{
    using System;
    using System.Collections.Generic;

    public interface ITutorialRunner
    {
        void RunAll(IInteractivity interactivity, IDictionary<String, Object> scope, Action<String> evaluate);
    }
}
