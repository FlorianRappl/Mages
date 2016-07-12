namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using Mages.Repl.Functions;
    using Mages.Repl.Plugins;
    using System.Collections.Generic;

    sealed class MagesCreator : IEngineCreator
    {
        private readonly IInteractivity _interactivity;
        private readonly IEnumerable<IModuleFileReader> _readers;

        public MagesCreator(IEnumerable<IModuleFileReader> readers, IInteractivity interactivity)
        {
            _readers = readers;
            _interactivity = interactivity;
        }

        public Engine CreateEngine()
        {
            var configuration = new Configuration
            {
                IsEngineExposed = true,
                IsEvalForbidden = false,
                IsThisAvailable = true,
            };
            var engine = new Engine(configuration);
            ReplFunctions.Integrate(engine, _interactivity);
            ReplPlugins.Integrate(engine);
            engine.AllowModules(_readers, this);
            return engine;
        }
    }
}
