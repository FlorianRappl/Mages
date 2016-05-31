namespace Mages.Repl.Functions
{
    using System;

    sealed class ReplObject
    {
        public String Read()
        {
            return Console.ReadLine();
        }

        public void Write(String str)
        {
            Console.Write(str);
        }

        public void WriteLine(String str)
        {
            Console.WriteLine(str);
        }
    }
}
