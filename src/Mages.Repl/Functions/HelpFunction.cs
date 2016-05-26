namespace Mages.Repl.Functions
{
    using Core;
    using System;
    using System.Collections.Generic;

    sealed class HelpFunction
    {
        private readonly IDictionary<String, Object> _globals;
        private readonly IInteractivity _interactivity;

        public HelpFunction(IInteractivity interactivity, IDictionary<String, Object> globals)
        {
            _interactivity = interactivity;
            _globals = globals;
        }

        internal static void AddTo(Engine engine, IInteractivity interactivity)
        {
            var function = new HelpFunction(interactivity, engine.Globals);
            engine.SetFunction("help", function.Invoke);
        }

        public Object Invoke(Object[] arguments)
        {
            if (arguments.Length == 0)
            {
                _interactivity.Write("Available functions: ");
                _interactivity.Write(Environment.NewLine);

                foreach (var global in _globals)
                {
                    if (global.Value is Function)
                    {
                        _interactivity.Write("   ");
                        _interactivity.Info(global.Key);
                        _interactivity.Write("()");
                        _interactivity.Write(Environment.NewLine);
                    }
                }
            }

            return null;
        }
    }
}
