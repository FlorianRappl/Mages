namespace Mages.Plugins.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    sealed class ModuleImporter
    {
        private readonly IEnumerable<IModuleFileReader> _readers;
        private readonly IEngineCreator _creator;

        public ModuleImporter(IEnumerable<IModuleFileReader> readers, IEngineCreator creator)
        {
            _readers = readers;
            _creator = creator;
        }

        public Object From(String fileName)
        {
            var path = default(String);

            foreach (var reader in _readers)
            {
                if (reader.TryGetPath(fileName, out path))
                {
                    var engine = Cache.Find(path);

                    if (engine == null)
                    {
                        var callback = reader.Prepare(path);

                        if (callback != null)
                        {
                            engine = _creator.CreateEngine();
                            Cache.Init(engine, path);
                            callback.Invoke(engine);
                        }
                    }

                    return Cache.Retrieve(engine);
                }
            }

            return null;
        }
    }
}