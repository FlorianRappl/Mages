namespace Mages.Repl.Installer
{
    using Squirrel;
    using System;
    using System.IO;
    using System.Linq;
    using System.Security;

    static class Actions
    {
        private static void CreateCmdFiles(string version)
        {
            var installDirectory = GetInstallDirectory();
            var exeFile = "mages.exe";

            var cmdContent = String.Format(@"@echo off{0}{1}\app-{2}\{3} %*", Environment.NewLine, installDirectory, version, exeFile);
            var cmdPath = Path.Combine(installDirectory, Path.ChangeExtension(exeFile, ".cmd"));
            File.WriteAllText(cmdPath, cmdContent);

            var promptContent = String.Format(@"@echo off{0}set PATH={1};%PATH%{0}@exit /B 0", Environment.NewLine, installDirectory);
            var promptPath = Path.Combine(installDirectory, "wyam.prompt.cmd");
            File.WriteAllText(promptPath, promptContent);
        }

        private static void CreateShortcuts()
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

        private static void RemoveShortcuts()
        {
            var startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var startMenuFolderPath = Path.Combine(startMenuPath, "Mages");

            if (Directory.Exists(startMenuFolderPath))
            {
                Directory.Delete(startMenuFolderPath, true);
            }
        }

        private static string GetInstallDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mages");
        }

        private static void Update()
        {
            using (var manager = GetUpdateManager())
            {
                Console.Write("Checking for updates... ");
                var updates = manager.CheckForUpdate().Result;
                var releases = updates.ReleasesToApply;

                if (releases.Count > 0)
                {
                    Console.Write("Downloading updates... ");
                    manager.DownloadReleases(releases).Wait();

                    Console.Write("Applying updates... ");
                    var version = manager.ApplyReleases(updates).Result;

                    Console.WriteLine("Successfully updated to version {0}", version);
                }
                else
                {
                    Console.WriteLine("No updates available");
                }
            }
        }

        private static UpdateManager GetUpdateManager()
        {
            return UpdateManager.GitHubUpdateManager("https://github.com/FlorianRappl/Mages").Result;
        }

        private static void AddPath()
        {
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            Console.WriteLine("Current PATH:");
            Console.WriteLine(currentPath);
            var installDirectory = GetInstallDirectory();
            var newPath = String.Concat(currentPath, ";", installDirectory);

            try
            {
                Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
                Console.WriteLine("New PATH:");
                Console.WriteLine(newPath);
            }
            catch (SecurityException)
            {
                Console.WriteLine("Failed to set new PATH due to security, you must run as administrator to change system environment variables.");
            }
        }

        private static void RemovePath()
        {
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            Console.WriteLine("Current PATH:");
            Console.WriteLine(currentPath);
            var paths = currentPath.Split(';').ToList();
            var installDirectory = GetInstallDirectory();
            var pathIndex = paths.IndexOf(installDirectory);

            if (pathIndex < 0)
            {
                Console.WriteLine("{0} was not found in PATH, no modifications made", installDirectory);
                return;
            }

            paths.RemoveAt(pathIndex);
            var newPath = String.Join(";", paths);

            try
            {
                Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
                Console.WriteLine("New PATH:");
                Console.WriteLine(newPath);
            }
            catch (SecurityException)
            {
                Console.WriteLine("Failed to set new PATH due to security, you must run as administrator to change system environment variables.");
            }
        }
    }
}
