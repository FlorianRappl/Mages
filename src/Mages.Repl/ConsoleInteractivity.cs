namespace Mages.Repl
{
    using System;

    sealed class ConsoleInteractivity : IInteractivity, IDisposable
    {
        public ConsoleInteractivity()
        {
            Console.CancelKeyPress += ConsoleCancelled;
        }

        public void Write(String output)
        {
            Console.Write(output);
        }

        public String Read()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("SWM> ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        public void Dispose()
        {
            Console.CancelKeyPress -= ConsoleCancelled;
        }

        private void ConsoleCancelled(Object sender, ConsoleCancelEventArgs e)
        {
            //e.Cancel = true;
        }
    }
}
