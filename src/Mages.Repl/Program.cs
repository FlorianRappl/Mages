namespace Mages.Repl
{
    using CommandLine;
    using Mages.Core;
    using System;
    using System.Diagnostics;
    using System.IO;

    static class Program
    {
        internal static void Main(String[] arguments)
        {
            Parser.Default.ParseArguments<Options>(arguments).WithParsed(Run);
        }

        private static void Run(Options options)
        {
            if (options.IsUpdating)
            {
                Process.Start("mages.installer.exe", "--update");
            }
            else
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
                else if (options.IsTutorial)
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
