namespace Mages.Repl
{
    using CommandLine;
    using System;

    sealed class Options
    {
        [Option("update", HelpText = "Checks for an application update.", Required = false)]
        public Boolean IsUpdating { get; set; }

        [Option("tutorial", HelpText = "Starts the application with a tutorial.", Required = false)]
        public Boolean IsTutorial { get; set; }

        [Value(0, HelpText = "The script file to use.", Required = false)]
        public String ScriptFile { get; set; }
    }
}
