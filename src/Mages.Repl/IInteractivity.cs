namespace Mages.Repl
{
    using System;

    interface IInteractivity
    {
        event EventHandler Cancelled;

        event EventHandler<KeyEventArgs> KeyPressed;

        void Write(String output);
    }
}
