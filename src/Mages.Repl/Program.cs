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
                var kernel = new StandardKernel(new ReplServices());
                var repl = kernel.Get<ReplCore>();

                if (!String.IsNullOrEmpty(options.ScriptFile))
                {
                    repl.Run(options.ScriptFile);
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
