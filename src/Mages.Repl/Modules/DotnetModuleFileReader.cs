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
            if (HasExtension(fileName))
            {
                if (Path.IsPathRooted(fileName))
                {
                    if (File.Exists(fileName))
                    {
                        path = fileName;
                        return true;
                    }
                }
                else
                {
                    var baseDirectories = new[]
                    {
                        Environment.CurrentDirectory,
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                    };

                    foreach (var baseDirectory in baseDirectories)
                    {
                        path = Path.Combine(baseDirectory, fileName);

                        if (File.Exists(path))
                        {
                            return true;
                        }
                    }

                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.NET", "assembly");
                    var results = Directory.GetFiles(path, fileName, SearchOption.AllDirectories);

                    if (results.Length > 0)
                    {
                        path = results[0];
                        return true;
                    }
                }
            }
                
            path = null;
            return false;
        }

        private static Boolean HasExtension(String fileName)
        {
            var ext = Path.GetExtension(fileName);
            return AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
        }
    }
}
