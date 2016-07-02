namespace Mages.Repl
{
    using Mages.Plugins.Modules;
    using Mages.Repl.Modules;
    using Ninject.Modules;

    sealed class ReplServices : NinjectModule
    {
        public override void Load()
        {
            Bind<IInteractivity>().ToConstant(new ConsoleInteractivity());
            Bind<IModuleFileReader>().ToConstant(new MagesModuleFileReader());
            Bind<IModuleFileReader>().ToConstant(new DotnetModuleFileReader());
            Bind<IModuleFileReader>().ToConstant(new NugetModuleFileReader());
            Bind<IFileReader>().To<OpenFileReader>();
            Bind<IEngineCreator>().To<MagesCreator>();
        }
    }
}
