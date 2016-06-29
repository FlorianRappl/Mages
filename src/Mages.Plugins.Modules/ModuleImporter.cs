namespace Mages.Plugins.Modules
{
    using System;
    using System.IO;

    sealed class ModuleImporter
    {
        private readonly IModuleFileReader _reader;
        private readonly IEngineCreator _creator;

        public ModuleImporter(IModuleFileReader reader, IEngineCreator creator)
        {
            _reader = reader;
            _creator = creator;
        }

        internal object From(String fileName)
        {
            var path = Path.GetFullPath(fileName);
            var engine = Cache.Find(path);

            if (engine == null)
            {
                var content = _reader.GetContent(path);

                if (!String.IsNullOrEmpty(content))
                {
                    engine = _creator.CreateEngine();
                    Cache.Init(engine, path);
                    engine.Interpret(content);
                }
            }

            return Cache.Retrieve(engine);
        }
    }
}