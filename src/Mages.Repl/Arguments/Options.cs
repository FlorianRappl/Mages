namespace Mages.Repl.Arguments
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

        [Value(0, HelpText = "The script file to use.", Required = false)]
        public String ScriptFile { get; set; }
    }
}
