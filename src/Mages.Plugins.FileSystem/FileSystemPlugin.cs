namespace Mages.Plugins.FileSystem
{
    using System;

    public static class FileSystemPlugin
    {
        public static readonly String Name = "FileSystem";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";

        public static Type File
        {
            get { return typeof(FileFunctions); }
        }

        public static Type Path
        {
            get { return typeof(PathFunctions); }
        }

        public static Type Dir
        {
            get { return typeof(DirectoryFunctions); }
        }

        public static String ToBase64(Byte[] content)
        {
            return Convert.ToBase64String(content);
        }

        public static Byte[] FromBase64(String content)
        {
            return Convert.FromBase64String(content);
        }
    }
}
