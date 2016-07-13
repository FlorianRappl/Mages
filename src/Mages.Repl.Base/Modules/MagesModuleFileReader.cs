namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using System;

    sealed class MagesModuleFileReader : IModuleFileReader
    {
        private static readonly String[] AllowedExtensions = new[] { ".ms", ".mages", ".txt", ".m" };
        private readonly IFileSystem _fs;

        public MagesModuleFileReader(IFileSystem fs)
        {
            _fs = fs;
        }

        public Action<Engine> Prepare(String path)
        {
            var content = _fs.RealText(path);
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
