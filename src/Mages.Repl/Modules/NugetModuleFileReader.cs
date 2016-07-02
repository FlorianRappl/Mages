namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using System;
    using System.IO;
    using System.Linq;

    sealed class NugetModuleFileReader : IModuleFileReader
    {
        private static readonly String[] AllowedExtensions = new[] { ".nupkg", ".nuget" };

        public Action<Engine> Prepare(String path)
        {
            throw new NotImplementedException();
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
