namespace Mages.Repl
{
    using System;

    interface IInteractivity
    {
        String Read();

        void Write(String output);
    }
}
