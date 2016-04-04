namespace Mages.Repl
{
    using System;

    sealed class ConsoleInteractivity : IInteractivity, IDisposable
    {
        public event EventHandler Cancelled;

        public event EventHandler<KeyEventArgs> KeyPressed;

        public ConsoleInteractivity()
        {
            Console.CancelKeyPress += ConsoleCancelled;
            InsertPrompt();
        }

        public void Write(String output)
        {
            Console.WriteLine(output);
            InsertPrompt();
        }

        public void Dispose()
        {
            Console.CancelKeyPress -= ConsoleCancelled;
        }

        private void ConsoleCancelled(Object sender, ConsoleCancelEventArgs e)
        {
            var handler = Cancelled;

            if (handler != null)
            {
                e.Cancel = true;
                handler.Invoke(sender, e);
            }
        }

        private void InsertPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("> ");
            Console.ResetColor();
        }
    }
}
