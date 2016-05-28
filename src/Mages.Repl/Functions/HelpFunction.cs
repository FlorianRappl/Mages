namespace Mages.Repl.Functions
{
    using Core;
    using System;
    using System.Collections.Generic;
    using System.Text;

    sealed class HelpFunction
    {
        private readonly IDictionary<String, Object> _globals;

        public HelpFunction(IDictionary<String, Object> globals)
        {
            _globals = globals;
        }

        internal static void AddTo(Engine engine)
        {
            var function = new HelpFunction(engine.Globals);
            engine.SetFunction("help", function.Invoke);
        }

        public Object Invoke(Object[] arguments)
        {
            if (arguments.Length == 0)
            {
                var sb = new StringBuilder();
                sb.Append("Available functions: ");

                foreach (var global in _globals)
                {
                    if (global.Value is Function)
                    {
                        sb.AppendLine();
                        sb.Append("   ");
                        sb.Append(global.Key);
                        sb.Append("()");
                    }
                }

                return sb.ToString();
            }

            return null;
        }
    }
}
