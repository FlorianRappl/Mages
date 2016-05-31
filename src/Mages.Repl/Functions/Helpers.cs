namespace Mages.Repl.Functions
{
    using Mages.Core;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    static class Helpers
    {
        public static Object Spawn(Function function, Object[] arguments)
        {
            var dict = new Dictionary<String, Object>
            {
                { "done", false },
                { "result", null },
                { "error", null }
            };
            Task.Factory.StartNew(() => function.Invoke(arguments)).ContinueWith(task =>
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

        public static String GetAllTopics(IDictionary<String, Object> globals)
        {
            var sb = new StringBuilder();
            sb.Append("Available functions: ");

            foreach (var global in globals)
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
    }
}
