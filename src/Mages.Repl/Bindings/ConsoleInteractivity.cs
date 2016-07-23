namespace Mages.Repl.Bindings
{
    using System;

    sealed class ConsoleInteractivity : IInteractivity
    {
        private const Int32 BufferSize = 4096;

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
            Console.ForegroundColor = ConsoleColor.Magenta;
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

            return ReadLine();
        }

        public void Info(String result)
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
                Console.Write(result);
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

        private static String ReadLine()
        {
            var newLineLength = Environment.NewLine.Length;
            var inputStream = Console.OpenStandardInput(4096);
            var bytes = new Byte[4096];
            var outputLength = inputStream.Read(bytes, 0, 4096);
            return Console.InputEncoding.GetString(bytes, 0, outputLength - newLineLength);
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
