namespace Mages.Repl
{
    using CommandLine;
    using Ninject;
    using System;
    using System.Diagnostics;

    static class Program
    {
        internal static readonly IKernel Kernel = new StandardKernel(new ReplServices());

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
                var repl = Kernel.Get<ReplCore>();

                if (!String.IsNullOrEmpty(options.ScriptFile))
                {
                    var fileReader = Kernel.Get<OpenFileReader>();
                    var content = fileReader.GetContent(options.ScriptFile);
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
