namespace Mages.Repl.Installer
{
    using CommandLine;
    using System;

    sealed class Options
    {
        [Option("squirrel-firstrun", Required = false)]
        public Boolean IsFirstRun { get; set; }

        [Option("squirrel-uninstall", Required = false)]
        public String UninstallVersion { get; set; }

        [Option("squirrel-obsolete", Required = false)]
        public String ObsoleteVersion { get; set; }

        [Option("squirrel-updated", Required = false)]
        public String UpdatedVersion { get; set; }

        [Option("squirrel-install", Required = false)]
        public String InstallVersion { get; set; }

        [Option("update", Required = false)]
        public Boolean IsUpdating { get; set; }
    }
}
