/* ****************************************
   Publishing workflow
   -------------------

 - Update CHANGELOG.md
 - Run a normal build with Cake
 - Push to devel and FF merge to main
 - Switch to main
 - Run a Publish build with Cake
 - Switch back to devel branch
   **************************************** */
#addin nuget:?package=Cake.FileHelpers&version=3.2.0
#addin nuget:?package=Octokit&version=0.32.0
using Octokit;

var isRunningOnGitHubActions = BuildSystem.GitHubActions.IsRunningOnGitHubActions;
var target = Argument("target", "Default");
var isPublish = target == "Publish";
var configuration = Argument("configuration", "Release");
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var releaseNotes = ParseReleaseNotes("./CHANGELOG.md");
var version = releaseNotes.Version.ToString();

if (isRunningOnGitHubActions)
{
    var buildNumber = BuildSystem.GitHubActions.Environment.Workflow.RunNumber;

    if (target == "Default")
    {
        version = $"{version}-ci-{buildNumber}";
    }
    else if (target == "PrePublish")
    {
        version = $"{version}-alpha-{buildNumber}";
    }
}

var buildDir = Directory("./src/Mages.Core/bin") + Directory(configuration) + Directory("netstandard2.0");
var replDir = Directory("./src/Mages.Repl/bin") + Directory(configuration) + Directory("netcoreapp3.1");
var installerDir = Directory("./src/Mages.Repl.Installer/bin") + Directory(configuration) + Directory("net45");
var buildResultDir = Directory("./bin") + Directory(version);
var nugetRoot = buildResultDir + Directory("nuget");
var chocolateyRoot = buildResultDir + Directory("chocolatey");
var squirrelRoot = buildResultDir + Directory("squirrel");
var squirrelBin = squirrelRoot + Directory("source");
var releaseDir = squirrelRoot + Directory("release");

// Initialization
// ----------------------------------------

Setup(context =>
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
        ReplaceRegexInFiles("./src/Directory.Build.props", "(?<=<Version>)(.+?)(?=</Version>)", version);
    });

Task("Build")
    .IsDependentOn("Restore-Packages")
    .IsDependentOn("Update-Assembly-Version")
    .Does(() =>
    {
        DotNetCoreBuild($"./src/Mages.sln", new DotNetCoreBuildSettings
        {
           Configuration = configuration,
        });
        
        DotNetCoreBuild($"./src/Mages.Repl/Mages.Repl.csproj", new DotNetCoreBuildSettings
        {
           Configuration = configuration,
        });
        
        DotNetCoreBuild($"./src/Mages.Repl.Installer/Mages.Repl.Installer.csproj", new DotNetCoreBuildSettings
        {
           Configuration = configuration,
        });
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration,
        };

        if (isRunningOnGitHubActions)
        {
            settings.Loggers.Add("GitHubActions");
        }

        DotNetCoreTest($"./src/Mages.Core.Tests/", settings);
        DotNetCoreTest($"./src/Mages.Repl.Tests/", settings);
    });

Task("Copy-Files")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var nugetBin = nugetRoot + Directory("lib") + Directory("netstandard2.0");
        CreateDirectory(nugetBin);
        CreateDirectory(squirrelBin);
        CreateDirectory(releaseDir);
        CopyFiles(new FilePath[]
        { 
            buildDir + File("Mages.Core.dll"),
            buildDir + File("Mages.Core.xml")
        }, nugetBin);
        CopyDirectory(replDir, squirrelBin);
        CopyDirectory(installerDir, squirrelBin);
        CopyFile("src/Mages.Nuget.nuspec", nugetRoot + File("Mages.nuspec"));
        CopyFile("src/Mages.Chocolatey.nuspec", chocolateyRoot + File("Mages.nuspec"));
        DeleteFiles(GetFiles(squirrelBin.Path.FullPath + "/*.pdb"));
        DeleteFiles(GetFiles(squirrelBin.Path.FullPath + "/*.vshost.*"));
    });

Task("Create-Nuget-Package")
    .IsDependentOn("Copy-Files")
    .Does(() =>
    {
        var nugetExe = GetFiles("./tools/**/nuget.exe").FirstOrDefault();

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
        var squirrelExe = GetFiles("./tools/**/squirrel.exe").FirstOrDefault();

        if (squirrelExe == null)
        {            
            throw new InvalidOperationException("Could not find squirrel.exe.");
        }

        var spec = squirrelRoot + File("Mages.nuspec");
        
        var setupIcon = GetFiles("./src/Mages.Repl.Installer/mages.ico").First().FullPath;
        
        StartProcess(squirrelExe, new ProcessSettings {
            Arguments = $"pack --packId \"Mages\" --packVersion \"{version}\" --allowUnaware --no-msi --silent --setupIcon \"{setupIcon}\" --packDirectory \"{squirrelBin}\" --releaseDir \"{releaseDir}\""
        });
    });

Task("Create-Chocolatey-Package")
    .IsDependentOn("Copy-Files")
    .WithCriteria(() => isRunningOnWindows)
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
    .WithCriteria(() => isRunningOnWindows)
    .Does(() => {
        var apiKey = EnvironmentVariable("CHOCOLATEY_API_KEY");
        var fileName = $"Mages.{version}.nupkg";
        var package = chocolateyRoot + File(fileName);

        if (String.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Could not resolve the Chocolatey API key.");
        }

        ChocolateyPush(package, new ChocolateyPushSettings
        { 
            Source = "https://chocolatey.org/",
            ApiKey = apiKey 
        });
    });
    
Task("Publish-GitHub-Release")
    .IsDependentOn("Publish-Packages")
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

        var newRelease = github.Repository.Release;
        var release = newRelease.Create("FlorianRappl", "Mages", new NewRelease("v" + version) 
        {
            Name = version,
            Body = String.Join(Environment.NewLine, releaseNotes.Notes),
            Prerelease = !isPublish,
            TargetCommitish = isPublish ? "main" : "devel"
        }).Result;

        var target = nugetRoot + Directory("lib") + Directory("netstandard2.0");
        var libPath = target + File("Mages.Core.dll");
        var releaseFiles = GetFiles(releaseDir.Path.FullPath + "/*");

        using (var libStream = System.IO.File.OpenRead(libPath.Path.FullPath))
        {
            newRelease.UploadAsset(release, new ReleaseAssetUpload("Mages.Core.dll", "application/x-msdownload", libStream, null)).Wait();
        }

        foreach (var file in releaseFiles)
        {
            var name = System.IO.Path.GetFileName(file.FullPath);

            if (name.Equals("Setup.exe"))
            {
                name = "Mages.exe";
            }

            using (var fileStream = System.IO.File.OpenRead(file.FullPath))
            {
                newRelease.UploadAsset(release, new ReleaseAssetUpload(name, "application/x-msdownload", fileStream, null)).Wait();
            }
        }
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

Task("Publish-Packages")
    .IsDependentOn("Default")
    .IsDependentOn("Publish-Nuget-Package")
    .IsDependentOn("Publish-Chocolatey-Package");

Task("Publish")
    .IsDependentOn("Publish-Packages")
    .IsDependentOn("Publish-GitHub-Release");

Task("PrePublish")
    .IsDependentOn("Publish-Packages")
    .IsDependentOn("Publish-GitHub-Release");

// Execution
// ----------------------------------------

RunTarget(target);
