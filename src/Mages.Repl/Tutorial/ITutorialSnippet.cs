namespace Mages.Repl.Tutorial
{
    using System;
    using System.Collections.Generic;

    interface ITutorialSnippet
    {
        String Title { get; }

        String Description { get; }

        String ExampleCommand { get; }

        String Task { get; }

        String Solution { get; }

        IEnumerable<String> Hints { get; }

        Boolean Check(IDictionary<String, Object> scope);
    }
}
