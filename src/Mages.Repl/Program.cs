namespace Mages.Repl
{
    using CommandLine;
    using Mages.Core;
    using Mages.Repl.Arguments;
    using System;
    using System.IO;

    static class Program
    {
        static void Main(String[] arguments)
        {
            Parser.Default.ParseArguments<Options>(arguments).WithParsed(Run);
        }

        private static void Run(Options options)
        {
            if (String.IsNullOrEmpty(options.UpdatedVersion) &&
                String.IsNullOrEmpty(options.UninstallVersion) &&
                String.IsNullOrEmpty(options.ObsoleteVersion) &&
                String.IsNullOrEmpty(options.InstallVersion))
            {
                var interactivity = new ConsoleInteractivity();
                var configuration = new Configuration
                {
                    IsEngineExposed = true,
                    IsEvalForbidden = false
                };
                var repl = new ReplCore(interactivity, configuration);

                if (!String.IsNullOrEmpty(options.ScriptFile))
                {
                    var content = File.ReadAllText(options.ScriptFile);
                    repl.Run(content);
                }
                else if (options.IsFirstRun)
                {
                    repl.Tutorial();
                }
                else
                {
                    repl.Run();
                }
            }
        }
    }
}
