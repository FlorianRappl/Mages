namespace Mages.Repl.Installer
{
    using CommandLine;
    using System;

    public static class Program
    {
        internal static void Main(String[] arguments)
        {
            Parser.Default.ParseArguments<Options>(arguments).WithParsed(Run);
        }

        private static void Run(Options options)
        {
            if (!String.IsNullOrEmpty(options.UpdatedVersion))
            {
                Actions.CreateCmdFile(options.UpdatedVersion);
            }
            else if (!String.IsNullOrEmpty(options.UninstallVersion))
            {
                Actions.RemoveShortcut();
                Actions.RemoveFromPath();
            }
            else if (!String.IsNullOrEmpty(options.ObsoleteVersion))
            {
            }
            else if (!String.IsNullOrEmpty(options.InstallVersion))
            {
                Actions.CreateCmdFile(options.InstallVersion);
                Actions.CreateShortcut();
                Actions.AddToPath();
            }
            else if (options.IsFirstRun)
            {
                Actions.CreateLatestCmdFile();
                Actions.CreateShortcut();
                Actions.AddToPath();
            }
            else if (options.IsUpdating)
            {
                Updater.PerformUpdate();
            }
            else
            {
                Mages.Repl.Program.Run();
            }
        }
    }
}
