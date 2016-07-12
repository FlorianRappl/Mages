namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using System;
    using System.IO;
    using System.Reflection;

    sealed class DotnetModuleFileReader : IModuleFileReader
    {
        private static readonly String[] AllowedExtensions = new[] { ".dll" };
        private static readonly String LibName = "__lib";

        public Action<Engine> Prepare(String path)
        {
            try
            {
                var lib = Assembly.LoadFile(path);
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
                    var windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                    var baseDir = Path.Combine(windows, "Microsoft.NET", "assembly");
                    var results = Directory.GetFiles(baseDir, fileName, SearchOption.AllDirectories);

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
