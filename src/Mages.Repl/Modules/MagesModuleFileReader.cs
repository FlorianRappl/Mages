namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using System;
    using System.IO;

    sealed class MagesModuleFileReader : IModuleFileReader
    {
        private static readonly String[] AllowedExtensions = new[] { ".ms", ".mages", ".txt", ".m" };

        public Action<Engine> Prepare(String path)
        {
            var content = File.ReadAllText(path);
            return engine => engine.Interpret(content);
        }

        public Boolean TryGetPath(String fileName, String directory, out String path)
        {
            path = null;
            return ModuleHelpers.HasExtension(AllowedExtensions, fileName) && 
                ModuleHelpers.TryFindPath(fileName, directory, out path);
        }
    }
}
