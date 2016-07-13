namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using System;
    using System.Reflection;

    sealed class DotnetModuleFileReader : IModuleFileReader
    {
        private static readonly String[] AllowedExtensions = new[] { ".dll" };
        private static readonly String LibName = "__lib";
        private readonly IFileSystem _fs;

        public DotnetModuleFileReader(IFileSystem fs)
        {
            _fs = fs;
        }

        public Action<Engine> Prepare(String path)
        {
            try
            {
                var content = _fs.ReadRaw(path);
                var lib = Assembly.Load(content);
                return engine => 
                {
                    engine.SetStatic(lib).WithName(LibName);
                    var export = engine.Globals["export"] as Function;
                    export.Call(engine.Globals[LibName]);
                };
            }
            catch
            {
                return null;
            }
        }

        public Boolean TryGetPath(String fileName, String directory, out String path)
        {
            if (ModuleHelpers.HasExtension(AllowedExtensions, fileName))
            {
                if (!ModuleHelpers.TryFindPath(fileName, directory, out path))
                {
                    var baseDir = _fs.GetGacDirectory();
                    var results = _fs.GetAllFiles(baseDir, fileName);

                    if (results.Length == 0)
                    {
                        return false;
                    }

                    path = results[0];
                }

                return true;
            }
                
            path = null;
            return false;
        }
    }
}
