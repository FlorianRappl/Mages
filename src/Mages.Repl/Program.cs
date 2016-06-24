namespace Mages.Repl
{
    using CommandLine;
    using Ninject;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Principal;

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
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                var path = Path.Combine(directory, "mages.installer.exe");
                var admin = IsAdministrator();
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(path, "--update") { UseShellExecute = !admin }
                };
                process.Start();
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

        private static Boolean IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
