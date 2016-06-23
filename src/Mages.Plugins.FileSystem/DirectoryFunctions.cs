namespace Mages.Plugins.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    static class DirectoryFunctions
    {
        public static Boolean Exists(String directoryName)
        {
            return Directory.Exists(directoryName);
        }

        public static void Create(String directoryName)
        {
            Directory.CreateDirectory(directoryName);
        }

        public static void Delete(String directoryName)
        {
            Directory.Delete(directoryName);
        }

        public static Object AllFiles(String directoryName)
        {
            var files = Directory.GetFiles(directoryName);
            var dict = new Dictionary<String, Object>();

            for (var i = 0; i < files.Length; i++)
            {
                dict[i.ToString()] = files[i];
            }

            return dict;
        }

        public static Object GetFiles(String directoryName, String pattern)
        {
            var files = Directory.GetFiles(directoryName, pattern);
            var dict = new Dictionary<String, Object>();

            for (var i = 0; i < files.Length; i++)
            {
                dict[i.ToString()] = files[i];
            }

            return dict;
        }
    }
}
