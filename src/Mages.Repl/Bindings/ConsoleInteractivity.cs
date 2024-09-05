namespace Mages.Repl.Bindings
{
    using System;
    using System.Collections.Generic;

    sealed class ConsoleInteractivity : IInteractivity
    {
        private readonly LineEditor _editor;
        private readonly List<CancellationRegistration> _blockers;
        private Boolean _warned;

        public event AutoCompleteHandler AutoComplete
        {
            add { _editor.AutoCompleteEvent += value; }
            remove { _editor.AutoCompleteEvent -= value; }
        }

        public ConsoleInteractivity()
        {
            var history = new History("Mages.Repl", 300);
            _editor = new LineEditor(history, OnCancelled);
            _blockers = new List<CancellationRegistration>();
            Console.TreatControlCAsInput = true;
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
            using (HandleCancellation(ShouldNotStop))
            {
                var result = _editor.Edit(prompt, String.Empty);

                if (result != null)
                {
                    _warned = false;
                }

                return result;
            }
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

        private Boolean ShouldNotStop()
        {
            var preventExit = _editor.AvailableText.Length > 0 || _editor.Prompt.StartsWith(" ");

            if (preventExit)
            {
                _warned = false;
                return true;
            }

            return WarnUserOrExit();
        }

        private Boolean WarnUserOrExit()
        {
            if (!_warned)
            {
                _warned = true;
                Console.WriteLine();
                Console.Write("(press ctrl + c again to exit)");
                return true;
            }

            return false;
        }

        public IDisposable HandleCancellation(Func<Boolean> shouldCancel)
        {
            return new CancellationRegistration(shouldCancel, _blockers);
        }

        private void OnCancelled()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("^C");
            Console.ResetColor();

            var allNegative = true;

            foreach (var blocker in _blockers)
            {
                if (blocker.ShouldCancel())
                {
                    allNegative = false;
                }
            }

            if (allNegative)
            {
                Environment.Exit(1);
            }
        }

        sealed class CancellationRegistration : IDisposable
        {
            private readonly Func<Boolean> _shouldCancel;
            private readonly List<CancellationRegistration> _list;

            public CancellationRegistration(Func<Boolean> shouldCancel, List<CancellationRegistration> list)
            {
                _shouldCancel = shouldCancel;
                _list = list;
                _list.Add(this);
            }

            public void Dispose()
            {
                _list.Remove(this);
            }

            public Boolean ShouldCancel()
            {
                return _shouldCancel.Invoke();
            }
        }
    }
}
