namespace Mages.Repl
{
    using Mages.Core.Runtime;
    using System;

    sealed class ConsoleInteractivity : IInteractivity, IDisposable
    {
        public ConsoleInteractivity()
        {
            Console.CancelKeyPress += ConsoleCancelled;
            IsPromptShown = true;
        }

        public Boolean IsPromptShown
        {
            get;
            set;
        }

        public void WritePrompt()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("SWM> ");
            Console.ResetColor();
        }

        public void Write(String output)
        {
            Console.Write(output);
        }

        public String Read()
        {
            if (IsPromptShown)
            {
                WritePrompt();
            }

            return Console.ReadLine();
        }

        public void Dispose()
        {
            Console.CancelKeyPress -= ConsoleCancelled;
        }

        private void ConsoleCancelled(Object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Environment.Exit(0);
        }

        public void Info(Object result)
        {
            if (result == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Undefined");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(Stringify.This(result));
                Console.ResetColor();
            }
        }

        public void Error(String message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ResetColor();
        }
    }
}
