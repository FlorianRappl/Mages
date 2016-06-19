namespace Mages.Modules.FileSystem
{
    using System;
    using System.IO;

    static class PathFunctions
    {
        public static String DirectoryName(String path)
        {
            return Path.GetDirectoryName(path);
        }

        public static String FileName(String path)
        {
            return Path.GetFileName(path);
        }

        public static String Extension(String path)
        {
            return Path.GetExtension(path);
        }

        public static String Root(String path)
        {
            return Path.GetPathRoot(path);
        }

        public static Boolean IsRelative(String path)
        {
            return !Path.IsPathRooted(path);
        }

        public static Boolean IsAbsolute(String path)
        {
            return Path.IsPathRooted(path);
        }

        public static String Combine(params String[] paths)
        {
            return Path.Combine(paths);
        }
    }
}
