namespace Mages.Modules.FileSystem
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

        public static Type Directory
        {
            get { return typeof(DirectoryFunctions); }
        }
    }
}
