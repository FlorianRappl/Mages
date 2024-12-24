namespace Mages.Repl.Tests.Mocks
{
    using System;

    sealed class PseudoInteractivity : IInteractivity
    {
#pragma warning disable 67
        public event AutoCompleteHandler AutoComplete;
#pragma warning restore 67

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
            OnError?.Invoke(message);
        }

        public IDisposable HandleCancellation(Func<Boolean> callback)
        {
            return null;
        }

        public void Info(String result)
        {
            OnInfo?.Invoke(result);
        }

        public String ReadLine()
        {
            return OnRead?.Invoke();
        }

        public void Write(String output)
        {
            OnWrite?.Invoke(output);
        }
    }
}
