namespace Mages.Repl
{
    using System;

    class Program
    {
        static void Main(String[] arguments)
        {
            var interactivity = new ConsoleInteractivity();
            var repl = new ReplCore(interactivity);
            repl.Run();
        }
    }
}
