namespace Mages.Repl
{
    using Mages.Plugins.Modules;
    using System;
    using System.IO;

    sealed class ModuleFileReader : IModuleFileReader
    {
        public String GetContent(String fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    return File.ReadAllText(fileName);
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
