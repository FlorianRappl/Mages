namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Repl.Functions;
    using Mages.Repl.Plugins;

    sealed class MagesCreator : IMagesCreator
    {
        public Engine CreateEngine()
        {
            var configuration = new Configuration
            {
                IsEngineExposed = true,
                IsEvalForbidden = false
            };
            var engine = new Engine(configuration);
            ReplFunctions.Integrate(engine);
            ReplPlugins.Integrate(engine);
            return engine;
        }
    }
}
