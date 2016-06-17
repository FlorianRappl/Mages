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
                    var content = ReadFile(options.ScriptFile, interactivity);
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

        private static String ReadFile(String fileName, IInteractivity interactivity)
        {
            var content = String.Empty;

            if (!File.Exists(fileName))
            {
                interactivity.Error(String.Format("The file {0} does not exist.", fileName));
                Environment.Exit(1);
            }

            try
            {
                content = File.ReadAllText(fileName);
            }
            catch
            {
                interactivity.Error(String.Format("Error while reading the file {0}.", fileName));
                Environment.Exit(1);
            }

            return content;
        }
    }
}
