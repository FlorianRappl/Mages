namespace Mages.Repl
{
    using Mages.Plugins.Modules;
    using System;
    using System.IO;

    sealed class ModuleFileReader : IModuleFileReader
    {
        public String GetContent(String path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return null;
            }
        }

        public Boolean TryGetPath(String fileName, out String path)
        {
            if (File.Exists(fileName))
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
    }
}
