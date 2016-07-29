namespace Mages.Repl
{
    using System;
    using System.Text;

    public class InputInfo
    {
        public InputInfo(ConsoleKeyInfo info, StringBuilder buffer, Int32 index)
        {
            Info = info;
            Buffer = buffer;
            Index = index;
        }

        public ConsoleKeyInfo Info
        {
            get;
            private set;
        }

        public StringBuilder Buffer
        {
            get;
            private set;
        }

        public Int32 Index
        {
            get;
            private set;
        }
    }
}
