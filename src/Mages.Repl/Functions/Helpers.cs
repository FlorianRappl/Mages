namespace Mages.Repl.Functions
{
    using Mages.Core;
    using Mages.Core.Ast;
    using Mages.Core.Ast.Walkers;
    using Mages.Core.Vm;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Mages.Core.Runtime.Converters;
    using System.Linq;

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

        public static String GetAllTopics(IDictionary<String, Object> globals, IDictionary<String, Object> scope)
        {
            var sb = new StringBuilder();
            sb.Append("Global Scope: ");
            Print(sb, scope);
            sb.AppendLine();
            sb.Append("Available API: ");
            Print(sb, globals);
            return sb.ToString();
        }

        private static void Print(StringBuilder sb, IDictionary<String, Object> items)
        {
            foreach (var item in items)
            {
                var name = item.Key;
                var type = item.Value.ToType();
                sb.AppendLine();
                sb.Append("- [");
                sb.Append(type);
                sb.Append("] ");
                sb.Append(name);
            }
        }

        public static Double Measure(Function f)
        {
            var sw = Stopwatch.StartNew();
            f.Invoke(new Object[0]);
            return sw.Elapsed.TotalMilliseconds;
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
            return sb.ToString(0, sb.Length - Environment.NewLine.Length);
        }
    }
}
