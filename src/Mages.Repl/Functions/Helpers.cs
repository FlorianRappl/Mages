namespace Mages.Repl.Functions
{
    using Mages.Core.Ast.Walkers;
    using Mages.Core;
    using Mages.Core.Ast;
    using Mages.Core.Vm;
    using Mages.Repl.Modules;
    using Ninject;
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

        public static Object Import(String fileName)
        {
            var path = Path.GetFullPath(fileName);
            var reader = Program.Kernel.Get<IFileReader>();
            var engine = Cache.Find(path);

            if (engine == null)
            {
                var content = reader.GetContent(path);

                if (!String.IsNullOrEmpty(content))
                {
                    var creator = Program.Kernel.Get<IMagesCreator>();
                    engine = creator.CreateEngine();
                    Cache.Init(engine, path);
                    engine.Interpret(content);
                }
            }

            return Cache.Retrieve(engine);
        }

        public static void Export(Engine engine, Object value)
        {
            Cache.Assign(engine, value);
        }
    }
}
