namespace Mages.Repl
{
    using System;

    interface IInteractivity
    {
        String Read();

        void Write(String output);

        void Info(Object result);

        void Error(String message);
    }
}
