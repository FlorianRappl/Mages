namespace Mages.Repl
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using Mages.Repl.Modules;

    static class ResolverExtensions
    {
        public static Engine CreateEngine(this IResolver resolver)
        {
            var readers = new IModuleFileReader[]
            {
                new MagesModuleFileReader(resolver.FileSystem),
                new DotnetModuleFileReader(resolver.FileSystem),
                new NugetModuleFileReader(resolver.FileSystem),
            };

            var creator = new MagesCreator(readers, resolver.Interactivity);
            return creator.CreateEngine();
        }
    }
}
