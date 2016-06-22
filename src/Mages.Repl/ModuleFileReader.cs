namespace Mages.Repl
{
    using System;
    using System.IO;

    sealed class ModuleFileReader : IFileReader
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
