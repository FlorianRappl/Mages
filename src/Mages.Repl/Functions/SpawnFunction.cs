namespace Mages.Repl.Functions
{
    using Mages.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    sealed class SpawnFunction
    {
        internal static void AddTo(Engine engine)
        {
            engine.SetFunction("spawn", SpawnFunction.Invoke);
        }

        public static Object Invoke(Object[] arguments)
        {
            if (arguments.Length > 0 && arguments[0] is Function)
            {
                var dict = new Dictionary<String, Object>
                {
                    { "done", false },
                    { "result", null },
                    { "error", null }
                };
                var function = (Function)arguments[0];
                var rest = arguments.Skip(1).ToArray();

                Task.Factory.StartNew(() => function.Invoke(rest)).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        dict["error"] = task.Exception.InnerException.Message;
                    }
                    else
                    {
                        dict["result"] = task.Result;
                    }

                    dict["done"] = true;
                });

                return dict;
            }

            return null;
        }
    }
}
