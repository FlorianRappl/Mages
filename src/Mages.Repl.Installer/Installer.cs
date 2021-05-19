namespace Mages.Repl.Installer
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security;

    static class Actions
    {
        public static void CreateLatestCmdFile()
        {
            var installDirectory = GetInstallDirectory();
            var subDirectories = Directory.GetDirectories(installDirectory, "app-*").Select(m => Path.GetFileName(m));
            var versions = subDirectories.Select(m => m.Substring(4)).OrderByDescending(m => m, StringComparer.OrdinalIgnoreCase);
            var version = versions.FirstOrDefault();
            CreateCmdFile(version);
        }

        public static void CreateCmdFile(String version)
        {
            var installDirectory = GetInstallDirectory();
            var exeFile = "mages.exe";
            var cmdContent = String.Format(@"@echo off{3}""{0}\app-{1}\{2}"" %* & exit", installDirectory, version, exeFile, Environment.NewLine);
            var cmdPath = Path.Combine(installDirectory, Path.ChangeExtension(exeFile, ".cmd"));
            File.WriteAllText(cmdPath, cmdContent);
        }

        public static void CreateShortcut()
        {
            var startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var startMenuFolderPath = Path.Combine(startMenuPath, "Mages");

            if (!Directory.Exists(startMenuFolderPath))
            {
                Directory.CreateDirectory(startMenuFolderPath);
            }

            var shortcutLocation = Path.Combine(startMenuFolderPath, "Mages.lnk");
            var shell = new IWshRuntimeLibrary.WshShell();
            var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);
            shortcut.TargetPath = "cmd.exe";
            shortcut.Arguments = "/k " + Path.Combine(GetInstallDirectory(), "mages.cmd");
            shortcut.Save();
        }

        public static void RemoveShortcut()
        {
            var startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var startMenuFolderPath = Path.Combine(startMenuPath, "Mages");

            if (Directory.Exists(startMenuFolderPath))
            {
                Directory.Delete(startMenuFolderPath, true);
            }
        }

        public static void Run()
        {
            var directory = GetInstallDirectory();
            Process.Start(Path.Combine(directory, "mages.exe"));
        }

        public static String GetInstallDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mages");
        }

        public static void AddToPath()
        {
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            var installDirectory = GetInstallDirectory();
            var newPath = String.Concat(currentPath, ";", installDirectory);

            try
            {
                Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
            }
            catch (SecurityException)
            {
                Console.WriteLine("Failed to set new PATH due to security, you must run as administrator to change system environment variables.");
            }
        }

        public static void RemoveFromPath()
        {
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            var paths = currentPath.Split(';').ToList();
            var installDirectory = GetInstallDirectory();
            var pathIndex = paths.IndexOf(installDirectory);

            if (pathIndex >= 0)
            {
                paths.RemoveAt(pathIndex);
                var newPath = String.Join(";", paths);

                try
                {
                    Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
                }
                catch (SecurityException)
                {
                    Console.WriteLine("Failed to set new PATH due to security, you must run as administrator to change system environment variables.");
                }
            }

        }
    }
}
