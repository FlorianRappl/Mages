namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using System;
    using System.IO;
    using System.Linq;
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

        public Boolean TryGetPath(String fileName, out String path)
        {
            path = null;

            if (HasExtension(fileName))
            {
                if (Path.IsPathRooted(fileName) && File.Exists(fileName))
                {
                    path = fileName;
                    return true;
                }
                else
                {
                    //TODO
                }
            }

            return false;
        }

        private static Boolean HasExtension(String fileName)
        {
            var ext = Path.GetExtension(fileName);
            return AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
        }
    }
}
