namespace Mages.Repl
{
    using System;

    sealed class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(Char key)
        {
            Key = key;
        }

        public Char Key 
        { 
            get; 
            private set; 
        }
    }
}
