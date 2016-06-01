/* ****************************************
   Publishing workflow
   -------------------

 - Update CHANGELOG.md
 - Run a normal build with Cake
 - Push to devel and FF merge to master
 - Switch to master
 - Run a Publish build with Cake
 - Switch back to devel branch
   **************************************** */

#addin "Cake.FileHelpers"
#addin "Octokit"
#addin "Cake.Squirrel"
using Octokit;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var isLocal = BuildSystem.IsLocalBuild;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
var buildNumber = AppVeyor.Environment.Build.Number;
var releaseNotes = ParseReleaseNotes("./CHANGELOG.md");
var version = releaseNotes.Version.ToString();
var buildDir = Directory("./src/Mages.Core/bin") + Directory(configuration);
var buildResultDir = Directory("./bin") + Directory(version);
var nugetRoot = buildResultDir + Directory("nuget");
var installerRoot = buildResultDir + Directory("installer");

// Initialization
// ----------------------------------------

Setup(() =>
{
    Information("Building version {0} of MAGES.", version);
    Information("For the publish target the following environment variables need to be set:");
    Information("  NUGET_API_KEY, GITHUB_API_TOKEN");
});

// Tasks
// ----------------------------------------

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[] { buildDir, buildResultDir, nugetRoot });
    });

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore("./src/Mages.sln");
    });

Task("Update-Assembly-Version")
    .Does(() =>
    {
        var file = Directory("./src") + File("SharedAssemblyInfo.cs");

        CreateAssemblyInfo(file, new AssemblyInfoSettings
        {
            Product = "Mages",
            Version = version,
            FileVersion = version,
            Company = "Polytrope",
            Copyright = String.Format("Copyright (c) {0}, Florian Rappl", DateTime.Now.Year)
        });
    });

Task("Build")
    .IsDependentOn("Restore-Packages")
    .IsDependentOn("Update-Assembly-Version")
    .Does(() =>
    {
        if (isRunningOnWindows)
        {
            MSBuild("./src/Mages.sln", new MSBuildSettings()
                .SetConfiguration(configuration)
                .SetVerbosity(Verbosity.Minimal)
            );
        }
        else
        {
            XBuild("./src/Mages.sln", new XBuildSettings()
                .SetConfiguration(configuration)
                .SetVerbosity(Verbosity.Minimal)
            );
        }
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new NUnit3Settings
        {
            Work = buildResultDir.Path.FullPath
        };

        if (isRunningOnAppVeyor)
        {
            settings.Where = "cat != ExcludeFromAppVeyor";
        }

        NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", settings);
    });

Task("Copy-Files")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var target = nugetRoot + Directory("lib") + Directory("net35");
        CreateDirectory(target);
        CopyFiles(new FilePath[]
        { 
            buildDir + File("Mages.Core.dll"),
            buildDir + File("Mages.Core.xml")
        }, target);
        CopyFiles(new FilePath[] { "src/Mages.nuspec" }, nugetRoot);
    });

Task("Create-Package")
    .IsDependentOn("Copy-Files")
    .Does(() =>
    {
        var nugetExe = GetFiles("./tools/**/nuget.exe").FirstOrDefault()
            ?? (isRunningOnAppVeyor ? GetFiles("C:\\Tools\\NuGet3\\nuget.exe").FirstOrDefault() : null);

        if (nugetExe == null)
        {            
            throw new InvalidOperationException("Could not find nuget.exe.");
        }
        
        var nuspec = nugetRoot + File("Mages.nuspec");
        
        NuGetPack(nuspec, new NuGetPackSettings
        {
            Version = version,
            OutputDirectory = nugetRoot,
            Symbols = false,
            Properties = new Dictionary<String, String> { { "Configuration", configuration } }
        });
    });
    
Task("Publish-Package")
    .IsDependentOn("Create-Package")
    .WithCriteria(() => isLocal)
    .Does(() =>
    {
        var apiKey = EnvironmentVariable("NUGET_API_KEY");

        if (String.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Could not resolve the NuGet API key.");
        }

        foreach (var nupkg in GetFiles(nugetRoot.Path.FullPath + "/*.nupkg"))
        {
            NuGetPush(nupkg, new NuGetPushSettings
            { 
                Source = "https://nuget.org/api/v2/package",
                ApiKey = apiKey 
            });
        }
    });

Task("Create-Installer")
    .IsDependentOn("Copy-Files")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() => {
        var nuspec = GetFiles("./src/Mages.Repl.Installer/Mages.nuspec").First();
        var pattern = String.Format("bin\\{0}\\**\\*", configuration);
        var packageDir = nuspec.GetDirectory() + ("/bin/" + configuration);

        NuGetPack(nuspec, new NuGetPackSettings
        {
            Version = version,
            BasePath = nuspec.GetDirectory(),
            OutputDirectory = packageDir,
            Symbols = false,
            Files = new [] { new NuSpecContent { Source = pattern, Target = "lib/net45" } }
        });

        var package = (packageDir + "/") + File("Mages." + version + ".nupkg");

        Squirrel(package, new SquirrelSettings
        {
            Silent = true,
            NoMsi = true,
            ReleaseDirectory = installerRoot,
            SetupIcon = GetFiles("./src/Mages.Repl.Installer/mages.ico").First().FullPath
        });

        DeleteFile(package);
    });
    
Task("Publish-Release")
    .IsDependentOn("Publish-Package")
    .WithCriteria(() => isLocal)
    .Does(() =>
    {
        var githubToken = EnvironmentVariable("GITHUB_API_TOKEN");

        if (String.IsNullOrEmpty(githubToken))
        {
            throw new InvalidOperationException("Could not resolve MAGES GitHub token.");
        }
        
        var github = new GitHubClient(new ProductHeaderValue("MagesCakeBuild"))
        {
            Credentials = new Credentials(githubToken)
        };

        var release = github.Release.Create("FlorianRappl", "Mages", new NewRelease("v" + version) 
        {
            Name = version,
            Body = String.Join(Environment.NewLine, releaseNotes.Notes),
            Prerelease = false,
            TargetCommitish = "master"
        }).Result;

        var target = nugetRoot + Directory("lib") + Directory("net35");
        var libPath = target + File("Mages.Core.dll");

        using (var libStream = System.IO.File.OpenRead(libPath.Path.FullPath))
        {
            github.Release.UploadAsset(release, new ReleaseAssetUpload("Mages.Core.dll", "application/x-msdownload", libStream, null)).Wait();
        }
    });
    
Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UpdateBuildVersion(version);
    });
    
// Targets
// ----------------------------------------
    
Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Create-Package");

Task("Default")
    .IsDependentOn("Package");    

Task("Publish")
    .IsDependentOn("Publish-Package")
    .IsDependentOn("Publish-Release");
    
Task("AppVeyor")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Update-AppVeyor-Build-Number");

// Execution
// ----------------------------------------

RunTarget(target);