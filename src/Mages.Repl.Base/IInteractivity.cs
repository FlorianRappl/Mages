namespace Mages.Repl
{
    using System;

    public interface IInteractivity
    {
        IDisposable HandleCancellation(Action callback);

        Boolean IsPromptShown { get; set; }

        void Prompt(String custom = null);

        String Read();

        void Write(String output);

        void Info(String result);

        void Error(String message);
    }
}
