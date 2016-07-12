namespace Mages.Repl.Functions
{
    using Mages.Core;
    using Mages.Core.Runtime;
    using Mages.Core.Runtime.Functions;
    using System;

    sealed class ConsoleFunctions
    {
        private readonly IInteractivity _interactivity;
        public readonly Function Read;
        public readonly Function Write;
        public readonly Function WriteLine;

        public ConsoleFunctions(IInteractivity interactivity)
        {
            _interactivity = interactivity;
            Read = new Function(args =>
            {
                return PerformRead();
            });
            Write = new Function(args =>
            {
                return Curry.MinOne(Write, args) ?? PerformWrite(args[0]);
            });
            WriteLine = new Function(args =>
            {
                return PerformWriteLine(args.Length == 0 ? String.Empty : args[0]);
            });
        }

        private Object PerformRead()
        {
            var current = _interactivity.IsPromptShown;
            _interactivity.IsPromptShown = false;
            var result = _interactivity.Read();
            _interactivity.IsPromptShown = current;
            return result;
        }

        private Object PerformWrite(Object value)
        {
            var str = Stringify.This(value);
            _interactivity.Write(str);
            return null;
        }

        private Object PerformWriteLine(Object value)
        {
            var str = Stringify.This(value);
            _interactivity.Write(str);
            _interactivity.Write(Environment.NewLine);
            return null;
        }
    }
}
