namespace Mages.Repl.Modules
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    static class ModuleHelpers
    {
        public static Boolean HasExtension(String[] extensions, String fileName)
        {
            var ext = Path.GetExtension(fileName);
            return extensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
        }

        public static Boolean TryFindPath(String fileName, String directory, out String path)
        {
            if (Path.IsPathRooted(fileName))
            {
                if (File.Exists(fileName))
                {
                    path = fileName;
                    return true;
                }
            }
            else
            {
                var baseDirectories = new[]
                {
                    directory,
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                };

                foreach (var baseDirectory in baseDirectories)
                {
                    path = Path.Combine(baseDirectory, fileName);

                    if (File.Exists(path))
                    {
                        return true;
                    }
                }
            }

            path = null;
            return false;
        }
    }
}
