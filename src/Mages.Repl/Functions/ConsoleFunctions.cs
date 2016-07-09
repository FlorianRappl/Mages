namespace Mages.Repl.Functions
{
    using Mages.Core;
    using Mages.Core.Runtime;
    using Mages.Core.Runtime.Functions;
    using System;

    static class ConsoleFunctions
    {
        public static readonly Function Read = new Function(args =>
        {
            return Console.ReadLine();
        });

        public static readonly Function Write = new Function(args =>
        {
            return Curry.MinOne(Write, args) ??
                PerformWrite(args[0]);
        });

        public static readonly Function WriteLine = new Function(args =>
        {
            return PerformWriteLine(args.Length == 0 ? String.Empty : args[0]);
        });

        private static Object PerformWrite(Object value)
        {
            var str = Stringify.This(value);
            Console.Write(str);
            return null;
        }

        private static Object PerformWriteLine(Object value)
        {
            var str = Stringify.This(value);
            Console.WriteLine(str);
            return null;
        }
    }
}
