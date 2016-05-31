namespace Mages.Repl.Functions
{
    using Mages.Core;
    using Mages.Core.Runtime.Functions;
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
            engine.SetConstant("process", new ProcessObject());
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
