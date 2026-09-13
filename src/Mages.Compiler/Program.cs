namespace Mages.Compiler
{
    using Mages.Repl;
    using System;

    public static class Program
    {
        internal static void Main(String[] arguments)
        {
            ReplRunner.Run(arguments);
        }

        public static void Run()
        {
            Main([]);
        }
    }
}
