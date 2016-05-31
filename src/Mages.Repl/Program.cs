namespace Mages.Repl
{
    using CommandLine;
    using Mages.Core;
    using Mages.Repl.Arguments;
    using Mages.Repl.Provisioning;
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
            if (!String.IsNullOrEmpty(options.UpdatedVersion))
            {
                Installer.CreateCmdFile(options.UpdatedVersion);
            }
            else if (!String.IsNullOrEmpty(options.UninstallVersion))
            {
                Installer.RemoveShortcut();
                Installer.RemoveFromPath();
            }
            else if (!String.IsNullOrEmpty(options.ObsoleteVersion))
            {
            }
            else if (!String.IsNullOrEmpty(options.InstallVersion))
            {
                Installer.CreateCmdFile(options.InstallVersion);
                Installer.CreateShortcut();
                Installer.AddToPath();
            }
            else if (options.IsUpdating)
            {
                Updater.PerformUpdate();
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
                //else if (options.IsFirstRun)
                //{
                //    repl.Tutorial();
                //}
                else
                {
                    repl.Run();
                }
            }
        }
    }
}
