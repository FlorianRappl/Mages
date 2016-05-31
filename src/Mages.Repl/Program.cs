namespace Mages.Repl
{
    using Core;
    using System;

    static class Program
    {
        static void Main(String[] arguments)
        {
            var interactivity = new ConsoleInteractivity();
            var configuration = new Configuration
            {
                IsEngineExposed = true,
                IsEvalForbidden = false
            };
            var repl = new ReplCore(interactivity, configuration);
            repl.Run();
        }
    }
}
