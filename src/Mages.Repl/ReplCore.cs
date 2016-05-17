namespace Mages.Repl
{
    using Core.Runtime;
    using Mages.Core;
    using System;

    sealed class ReplCore
    {
        private readonly IInteractivity _interactivity;
        private readonly Engine _engine;

        public ReplCore(IInteractivity interactivity)
        {
            _interactivity = interactivity;
            _engine = new Engine();
        }

        public void Run()
        {
            var input = default(String);

            do
            {
                input = _interactivity.Read();

                if (!String.IsNullOrEmpty(input))
                {
                    var result = _engine.Interpret(input);
                    _interactivity.Write(Stringify.This(result));
                    _interactivity.Write(Environment.NewLine);
                }
            }
            while (input != null);
        }
    }
}
