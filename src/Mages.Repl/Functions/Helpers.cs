namespace Mages.Repl.Functions
{
    using Core.Ast.Walkers;
    using Mages.Core;
    using Mages.Core.Ast;
    using Mages.Core.Vm;
    using System;
    using System.Collections.Generic;
    using System.IO;
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

        public static String ShowIl(Engine engine, String source)
        {
            var tokens = source.ToTokenStream();
            var statements = engine.Parser.ParseStatements(tokens);
            var operations = statements.MakeRunnable();
            return operations.Serialize();
        }

        public static String ShowAst(Engine engine, String source)
        {
            var tokens = source.ToTokenStream();
            var statements = engine.Parser.ParseStatements(tokens);
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var walker = new SerializeTreeWalker(writer);
            statements.ToBlock().Accept(walker);
            return sb.ToString();
        }
    }
}
