namespace Mages.Plugins.Modules
{
    using Mages.Core;
    using System;
    using System.IO;

    static class Variables
    {
        private static readonly String PathKey = "__path";
        private static readonly String DefaultPath = Path.Combine(Environment.CurrentDirectory, "Repl.ms");

        public static String GetPath(this Engine engine)
        {
            var value = default(Object);
            engine.Globals.TryGetValue(PathKey, out value);
            return value as String;
        }

        public static String GetDirectory(this Engine engine)
        {
            var path = engine.GetPath();
            return Path.GetDirectoryName(path ?? DefaultPath);
        }

        public static void SetPath(this Engine engine, String value)
        {
            engine.Globals[PathKey] = value;
        }
    }
}
