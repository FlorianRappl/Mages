namespace Mages.Repl.Tests.Mocks
{
    using System;

    sealed class PseudoInteractivity : IInteractivity
    {
        public event AutoCompleteHandler AutoComplete;

        public Action<String> OnError
        {
            get;
            set;
        }

        public Action<String> OnInfo
        {
            get;
            set;
        }

        public Func<String> OnRead
        {
            get;
            set;
        }

        public Action<String> OnWrite
        {
            get;
            set;
        }

        public String GetLine(String prompt)
        {
            Write(prompt ?? String.Empty);
            return ReadLine();
        }

        public void Error(String message)
        {
            var handler = OnError;

            if (handler != null)
            {
                handler.Invoke(message);
            }
        }

        public IDisposable HandleCancellation(Action callback)
        {
            return null;
        }

        public void Info(String result)
        {
            var handler = OnInfo;

            if (handler != null)
            {
                handler.Invoke(result);
            }
        }

        public String ReadLine()
        {
            var handler = OnRead;

            if (handler != null)
            {
                return handler.Invoke();
            }

            return null;
        }

        public void Write(String output)
        {
            var handler = OnWrite;

            if (handler != null)
            {
                handler.Invoke(output);
            }
        }
    }
}
