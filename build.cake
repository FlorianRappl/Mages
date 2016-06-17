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
var installerDir = Directory("./src/Mages.Repl.Installer/bin") + Directory(configuration);
var buildResultDir = Directory("./bin") + Directory(version);
var nugetRoot = buildResultDir + Directory("nuget");
var chocolateyRoot = buildResultDir + Directory("chocolatey");
var squirrelRoot = buildResultDir + Directory("squirrel");

// Initialization
// ----------------------------------------

Setup(() =>
{
    Information("Building version {0} of MAGES.", version);
    Information("For the publish target the following environment variables need to be set:");
    Information("* NUGET_API_KEY");
    Information("* CHOCOLATEY_API_KEY");
    Information("* GITHUB_API_TOKEN");
});

// Tasks
// ----------------------------------------

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[] { buildDir, installerDir, buildResultDir, nugetRoot, chocolateyRoot, squirrelRoot });
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
        var nugetBin = nugetRoot + Directory("lib") + Directory("net35");
        var squirrelBin = squirrelRoot + Directory("lib") + Directory("net45");
        CreateDirectory(nugetBin);
        CreateDirectory(squirrelBin);
        CopyFiles(new FilePath[]
        { 
            buildDir + File("Mages.Core.dll"),
            buildDir + File("Mages.Core.xml")
        }, nugetBin);
        CopyDirectory(installerDir, squirrelBin);
        CopyFile("src/Mages.Nuget.nuspec", nugetRoot + File("Mages.nuspec"));
        CopyFile("src/Mages.Chocolatey.nuspec", chocolateyRoot + File("Mages.nuspec"));
        CopyFile("src/Mages.Squirrel.nuspec", squirrelRoot + File("Mages.nuspec"));
        DeleteFiles(GetFiles(squirrelBin.Path.FullPath + "/*.pdb"));
        DeleteFiles(GetFiles(squirrelBin.Path.FullPath + "/*.vshost.*"));
    });

Task("Create-Nuget-Package")
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
    
Task("Publish-Nuget-Package")
    .IsDependentOn("Create-Nuget-Package")
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

Task("Create-Squirrel-Package")
    .IsDependentOn("Copy-Files")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() => {
        var nugetExe = GetFiles("./tools/**/nuget.exe").FirstOrDefault()
            ?? (isRunningOnAppVeyor ? GetFiles("C:\\Tools\\NuGet3\\nuget.exe").FirstOrDefault() : null);

        if (nugetExe == null)
        {            
            throw new InvalidOperationException("Could not find nuget.exe.");
        }

        var spec = squirrelRoot + File("Mages.nuspec");
        var release = squirrelRoot + Directory("release");
        CreateDirectory(release);

        NuGetPack(spec, new NuGetPackSettings
        {
            Version = version,
            BasePath = squirrelRoot,
            OutputDirectory = squirrelRoot,
            Symbols = false
        });

        var fileName = "Mages." + version + ".nupkg";
        var package = squirrelRoot + File(fileName);
        
        Squirrel(package, new SquirrelSettings
        {
            Silent = true,
            NoMsi = true,
            ReleaseDirectory = release,
            SetupIcon = GetFiles("./src/Mages.Repl.Installer/mages.ico").First().FullPath
        });
    });

Task("Create-Chocolatey-Package")
    .IsDependentOn("Copy-Files")
    .WithCriteria(() => isLocal)
    .Does(() => {
        var content = String.Format("$packageName = 'Mages'{1}$installerType = 'exe'{1}$url32 = 'https://github.com/FlorianRappl/Mages/releases/download/v{0}/Mages.exe'{1}$silentArgs = ''{1}{1}Install-ChocolateyPackage \"$packageName\" \"$installerType\" \"$silentArgs\" \"$url32\"", version, Environment.NewLine);
        var nuspec = chocolateyRoot + File("Mages.nuspec");
        var toolsDirectory = chocolateyRoot + Directory("tools");
        var scriptFile = toolsDirectory + File("chocolateyInstall.ps1");

        CreateDirectory(toolsDirectory);
        System.IO.File.WriteAllText(scriptFile.Path.FullPath, content);
        
        ChocolateyPack(nuspec, new ChocolateyPackSettings
        {
            Version = version,
            OutputDirectory = chocolateyRoot
        });
    });

Task("Publish-Chocolatey-Package")
    .IsDependentOn("Create-Chocolatey-Package")
    .WithCriteria(() => isLocal)
    .Does(() => {
        var apiKey = EnvironmentVariable("CHOCOLATEY_API_KEY");
        var fileName = "Mages." + version + ".nupkg";
        var package = chocolateyRoot + File(fileName);

        if (String.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Could not resolve the Chocolatey API key.");
        }

        ChocolateyPush(package, new ChocolateyPushSettings
        { 
            Source = "https://chocolatey.org/packages",
            ApiKey = apiKey 
        });
    });
    
Task("Publish-GitHub-Release")
    .IsDependentOn("Create-Squirrel-Package")
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
        var setupPath = squirrelRoot + Directory("release") + File("Setup.exe");

        using (var libStream = System.IO.File.OpenRead(libPath.Path.FullPath))
        {
            github.Release.UploadAsset(release, new ReleaseAssetUpload("Mages.Core.dll", "application/x-msdownload", libStream, null)).Wait();
        }

        using (var setupStream = System.IO.File.OpenRead(setupPath.Path.FullPath))
        {
            github.Release.UploadAsset(release, new ReleaseAssetUpload("Mages.exe", "application/x-msdownload", setupStream, null)).Wait();
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
    .IsDependentOn("Create-Squirrel-Package")
    .IsDependentOn("Create-Chocolatey-Package")
    .IsDependentOn("Create-Nuget-Package");

Task("Default")
    .IsDependentOn("Package");    

Task("Publish")
    .IsDependentOn("Publish-Nuget-Package")
    .IsDependentOn("Publish-GitHub-Release")
    .IsDependentOn("Publish-Chocolatey-Package");
    
Task("AppVeyor")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Update-AppVeyor-Build-Number");

// Execution
// ----------------------------------------

RunTarget(target);