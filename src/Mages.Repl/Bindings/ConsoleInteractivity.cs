namespace Mages.Repl.Bindings
{
    using System;

    sealed class ConsoleInteractivity : IInteractivity
    {
        private const Int32 BufferSize = 4096;
        private readonly LineEditor _editor;

        public event AutoCompleteHandler AutoComplete
        {
            add { _editor.AutoCompleteEvent += value; }
            remove { _editor.AutoCompleteEvent -= value; }
        }

        public ConsoleInteractivity()
        {
            var history = new History("Mages.Repl", 300);
            _editor = new LineEditor(history);
        }

        public void Write(String output)
        {
            Console.Write(output);
        }

        public String ReadLine()
        {
            return Console.ReadLine();
        }

        public String GetLine(String prompt)
        {
            return _editor.Edit(prompt, String.Empty);
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

        public IDisposable HandleCancellation(Func<Boolean> shouldCancel)
        {
            return new CancellationRegistration(shouldCancel);
        }

        struct CancellationRegistration : IDisposable
        {
            private readonly Func<Boolean> _shouldCancel;

            public CancellationRegistration(Func<Boolean> shouldCancel)
            {
                _shouldCancel = shouldCancel;
                Console.CancelKeyPress += OnCancelled;
            }

            public void Dispose()
            {
                Console.CancelKeyPress -= OnCancelled;
            }

            private void OnCancelled(Object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = _shouldCancel.Invoke();
            }
        }
    }
}
