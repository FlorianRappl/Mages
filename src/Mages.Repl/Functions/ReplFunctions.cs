namespace Mages.Repl.Functions
{
    using Mages.Core;
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Linq;

    static class ReplFunctions
    {
        public static void Integrate(Engine engine)
        {
            engine.SetFunction("spawn", new Function(args =>
            {
                var id = engine.Globals["spawn"] as Function;
                return Curry.MinOne(id, args) ??
                    If.Is<Function>(args, f => Helpers.Spawn(f, args.Skip(1).ToArray()));
            }));
            engine.SetFunction("il", new Function(args =>
            {
                var id = engine.Globals["il"] as Function;
                return Curry.MinOne(id, args) ??
                    If.Is<String>(args, source => Helpers.ShowIl(engine, source));
            }));
            engine.SetFunction("ast", new Function(args =>
            {
                var id = engine.Globals["ast"] as Function;
                return Curry.MinOne(id, args) ??
                    If.Is<String>(args, source => Helpers.ShowAst(engine, source));
            }));
            engine.SetConstant("process", new ProcessObject());
            engine.SetConstant("measure", new Function(args =>
            {
                var id = engine.Globals["measure"] as Function;
                return Curry.MinOne(id, args) ??
                    If.Is<Function>(args, f => Helpers.Measure(f));
            }));
            engine.SetFunction("help", new Function(args =>
            {
                if (args.Length == 0)
                {
                    return Helpers.GetAllTopics(engine.Globals);
                }

                return null;
            }));
            engine.SetConstant("repl", new ReplObject());
        }
    }
}
