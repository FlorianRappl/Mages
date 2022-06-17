namespace Mages.Repl.Installer
{
    using Squirrel;
    using System;

#pragma warning disable CA1416 // Validate platform compatibility
    static class Updater
    {
        public static void PerformUpdate()
        {
            using (var manager = GetUpdateManager())
            {
                Console.WriteLine("Checking for updates... ");
                var updates = manager.CheckForUpdate().Result;
                var releases = updates.ReleasesToApply;

                if (releases.Count > 0)
                {
                    Console.WriteLine("Downloading updates... ");
                    manager.DownloadReleases(releases).Wait();

                    Console.WriteLine("Applying updates... ");
                    var version = manager.ApplyReleases(updates).Result;

                    Console.WriteLine("Successfully updated to version {0}", version);
                }
                else
                {
                    Console.WriteLine("No updates available");
                }
            }
        }

        private static UpdateManager GetUpdateManager() => new GithubUpdateManager("https://github.com/FlorianRappl/Mages", false, null, "Mages");
    }
#pragma warning restore CA1416 // Validate platform compatibility
}
