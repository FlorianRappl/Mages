namespace Mages.Repl
{
    using Mages.Core.Runtime;
    using System;

    sealed class ConsoleInteractivity : IInteractivity
    {
        public ConsoleInteractivity()
        {
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

        public IDisposable HandleCancellation(Action callback)
        {
            return new CancellationRegistration(callback);
        }

        struct CancellationRegistration : IDisposable
        {
            private readonly Action _callback;

            public CancellationRegistration(Action callback)
            {
                _callback = callback;
                Console.CancelKeyPress += OnCancelled;
            }

            public void Dispose()
            {
                Console.CancelKeyPress -= OnCancelled;
            }

            private void OnCancelled(Object sender, ConsoleCancelEventArgs e)
            {
                _callback.Invoke();
                e.Cancel = true;
            }
        }
    }
}
