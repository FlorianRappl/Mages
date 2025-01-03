﻿namespace Mages.Repl.Modules
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    sealed class NugetModuleFileReader : IModuleFileReader
    {
        private static readonly String LibName = "__lib";
        private static readonly String[] AllowedExtensions = [".nupkg", ".nuget", ".pkg"];
        private readonly Mages.Repl.IFileSystem _fs;

        public NugetModuleFileReader(Mages.Repl.IFileSystem fs)
        {
            _fs = fs;
        }

        public Action<Engine> Prepare(String path)
        {
            var package = GetPackage(path);

            if (package is not null)
            {
                var files = package.GetLibFiles().ToList();
                return engine => ExposeLibrary(files, engine);
            }

            return null;
        }

        public Boolean TryGetPath(String fileName, String directory, out String path)
        {
            if (ModuleHelpers.HasExtension(AllowedExtensions, fileName))
            {
                if (ModuleHelpers.TryFindPath(fileName, directory, out path))
                {
                    return true;
                }
                else if (_fs.GetDirectory(fileName).Length == 0)
                {
                    path = fileName;
                    return true;
                }
            }

            path = null;
            return false;
        }

        private IPackage GetPackage(String path)
        {
            if (_fs.IsRelative(path))
            {
                var info = ParsePackageInfo(path);
                var manager = GetPackageManager();
                var package = manager.LocalRepository.FindPackage(info.Name, info.Version);

                if (package is null)
                {
                    package = manager.SourceRepository.FindPackage(info.Name, info.Version);
                    manager.LocalRepository.AddPackage(package);
                }

                return package;
            }

            return new ZipPackage(path);
        }

        private PackageManager GetPackageManager()
        {
            var repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var baseDirectory = _fs.GetDirectory(Assembly.GetExecutingAssembly().Location);
            var packageDirectory = _fs.GetPath(baseDirectory, "packages");
            return new PackageManager(repository, packageDirectory);
        }

        private PackageInfo ParsePackageInfo(String path)
        {
            var packageId = _fs.GetFileName(path);
            var parts = packageId.Split('.');
            var version = default(SemanticVersion);
            var offset = parts[0].Length + 1;

            for (var i = 1; i < parts.Length; i++)
            {
                var result = 0;

                if (Int32.TryParse(parts[i], out result) && result >= 0)
                {
                    var rest = packageId.Substring(offset);

                    if (SemanticVersion.TryParse(rest, out version))
                    {
                        packageId = packageId.Substring(0, offset - 1);
                        break;
                    }
                }

                offset += parts[i].Length + 1;
            }

            return new PackageInfo { Name = packageId, Version = version };
        }

        private static void ExposeLibrary(IEnumerable<IPackageFile> files, Engine engine)
        {
            if (TryInsertLibrary(files, engine))
            {
                var export = engine.Globals["export"] as Function;
                export.Call(engine.Globals[LibName]);
            }
        }

        private static Boolean TryInsertLibrary(IEnumerable<IPackageFile> files, Engine engine)
        {
            foreach (var file in files)
            {
                if (file.TargetFramework.Version.CompareTo(Version.Parse("4.5")) <= 0)
                {
                    using (var content = new MemoryStream())
                    {
                        using (var stream = file.GetStream())
                        {
                            stream.CopyTo(content);
                        }

                        var lib = Assembly.Load(content.ToArray());
                        engine.SetStatic(lib).WithName(LibName);
                    }

                    return true;
                }
            }

            return false;
        }

        struct PackageInfo
        {
            public String Name;
            public SemanticVersion Version;
        }
    }
}
