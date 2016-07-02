namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using System;
    using System.IO;
    using System.Linq;

    sealed class MagesModuleFileReader : IModuleFileReader
    {
        private static readonly String[] AllowedExtensions = new[] { ".ms", ".mages", ".txt", ".m" };

        public Action<Engine> Prepare(String path)
        {
            try
            {
                var content = File.ReadAllText(path);
                return engine => engine.Interpret(content);
            }
            catch
            {
                return null;
            }
        }

        public Boolean TryGetPath(String fileName, out String path)
        {
            if (HasExtension(fileName) && File.Exists(fileName))
            {
                path = Path.GetFullPath(fileName);
                return true;
            }
            else
            {
                path = null;
                return false;
            }
        }

        private static Boolean HasExtension(String fileName)
        {
            var ext = Path.GetExtension(fileName);
            return AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
        }
    }
}
