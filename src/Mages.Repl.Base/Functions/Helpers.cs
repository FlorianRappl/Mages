﻿namespace Mages.Repl.Functions
{
    using Mages.Core;
    using Mages.Core.Ast;
    using Mages.Core.Ast.Walkers;
    using Mages.Core.Vm;
    using Mages.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    static class Helpers
    {
        public static IDictionary<String, Object> Spawn(Function function, Object[] arguments)
        {
            return Task.Run(() => function.Invoke(arguments)).AsFuture();
        }

        public static IDictionary<String, Object> Sleep(Double time)
        {
            return Task.Delay(TimeSpan.FromMilliseconds(time)).AsFuture();
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

        public static String ShowTokens(Engine engine, String source)
        {
            var tokens = source.ToTokenStream();
            var list = new List<String>();

            while (tokens.MoveNext())
            {
                list.Add($"{tokens.Current}");
            }

            return String.Join(Environment.NewLine, list);
        }
    }
}
