namespace Mages.Repl
{
    using System;
    using System.IO;

    sealed class RealFileSystem : IFileSystem
    {
        public String[] GetAllFiles(String baseDir, String fileName)
        {
            return Directory.GetFiles(baseDir, fileName, SearchOption.AllDirectories);
        }

        public String GetDirectory(String path)
        {
            return Path.GetDirectoryName(path);
        }

        public String GetFileName(String path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public String GetGacDirectory()
        {
            var windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            return Path.Combine(windows, "Microsoft.NET", "assembly");
        }

        public String GetPath(params String[] paths)
        {
            return Path.Combine(paths);
        }

        public Boolean IsRelative(String path)
        {
            return !Path.IsPathRooted(path);
        }

        public Byte[] ReadRaw(String path)
        {
            return File.ReadAllBytes(path);
        }

        public String RealText(String path)
        {
            return File.ReadAllText(path);
        }
    }
}
