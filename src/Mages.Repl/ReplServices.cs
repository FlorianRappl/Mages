namespace Mages.Repl
{
    using Mages.Plugins.Modules;
    using Ninject.Modules;

    sealed class ReplServices : NinjectModule
    {
        public override void Load()
        {
            Bind<IInteractivity>().ToConstant(new ConsoleInteractivity());
            Bind<IModuleFileReader>().ToConstant(new ModuleFileReader());
            Bind<IFileReader>().To<OpenFileReader>();
            Bind<IEngineCreator>().To<MagesCreator>();
        }
    }
}
