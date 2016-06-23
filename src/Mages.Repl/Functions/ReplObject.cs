namespace Mages.Repl.Functions
{
    using Mages.Core.Runtime;
    using System;

    sealed class ReplObject
    {
        public String Read()
        {
            return Console.ReadLine();
        }

        public void Write(Object value)
        {
            var str = Stringify.This(value);
            Console.Write(str);
        }

        public void WriteLine(Object value)
        {
            var str = Stringify.This(value);
            Console.WriteLine(str);
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}
