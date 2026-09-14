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

var buildDir = Directory("./src/Mages.Core/bin") + Directory(configuration) + Directory("netstandard2.1");
var compilerDir = Directory("./src/Mages.Compiler/bin") + Directory(configuration) + Directory("net10.0");
var buildResultDir = Directory("./bin") + Directory(version);

// Initialization
// ----------------------------------------

Setup(context =>
{
    Information("Building version {0} of MAGES.", version);
    Information("For the publish target the following environment variables need to be set:");
    Information("* NUGET_API_KEY (short-lived key from NuGet trusted publishing)");
    Information("* GITHUB_API_TOKEN");
});

// Tasks
// ----------------------------------------

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[] { buildDir, buildResultDir });
    });

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore("./src/Mages.slnx");
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
        DotNetCoreBuild($"./src/Mages.slnx", new DotNetCoreBuildSettings
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
        var nugetBin = buildResultDir + Directory("lib") + Directory("netstandard2.1");
        CreateDirectory(nugetBin);
        CopyFiles(new FilePath[]
        { 
            buildDir + File("Mages.Core.dll"),
            buildDir + File("Mages.Core.xml")
        }, nugetBin);
        CopyFile("README.md", buildResultDir + File("README.md"));
    });

Task("Create-Nuget-Package")
    .IsDependentOn("Copy-Files")
    .Does(() =>
    {
        DotNetCorePack("./src/Mages.Core/Mages.Core.csproj", new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = buildResultDir,
            ArgumentCustomization = args => args.Append($"/p:Version={version}")
        });

        DotNetCorePack("./src/Mages.Compiler/Mages.Compiler.csproj", new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = buildResultDir,
            ArgumentCustomization = args => args.Append($"/p:Version={version}")
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

        foreach (var nupkg in GetFiles(buildResultDir.Path.FullPath + "/*.nupkg"))
        {
            DotNetCoreNuGetPush(nupkg.FullPath, new DotNetCoreNuGetPushSettings
            { 
                Source = "https://api.nuget.org/v3/index.json",
                ApiKey = apiKey 
            });
        }
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

        var target = buildResultDir + Directory("lib") + Directory("netstandard2.1");
        var libPath = target + File("Mages.Core.dll");

        using (var libStream = System.IO.File.OpenRead(libPath.Path.FullPath))
        {
            newRelease.UploadAsset(release, new ReleaseAssetUpload("Mages.Core.dll", "application/x-msdownload", libStream, null)).Wait();
        }
    });
    
// Targets
// ----------------------------------------
    
Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Create-Nuget-Package");

Task("Default")
    .IsDependentOn("Package");

Task("Publish-Packages")
    .IsDependentOn("Default")
    .IsDependentOn("Publish-Nuget-Package");

Task("Publish")
    .IsDependentOn("Publish-Packages")
    .IsDependentOn("Publish-GitHub-Release");

Task("PrePublish")
    .IsDependentOn("Publish-Packages")
    .IsDependentOn("Publish-GitHub-Release");

// Execution
// ----------------------------------------

RunTarget(target);
