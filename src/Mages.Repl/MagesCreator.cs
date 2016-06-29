namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using Mages.Repl.Functions;
    using Mages.Repl.Plugins;

    sealed class MagesCreator : IEngineCreator
    {
        private readonly IModuleFileReader _reader;

        public MagesCreator(IModuleFileReader reader)
        {
            _reader = reader;
        }

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
            engine.AllowModules(_reader, this);
            return engine;
        }
    }
}
