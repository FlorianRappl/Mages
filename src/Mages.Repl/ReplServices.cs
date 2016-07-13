namespace Mages.Repl
{
    using Mages.Repl.Bindings;
    using Ninject.Modules;

    sealed class ReplServices : NinjectModule
    {
        public override void Load()
        {
            Bind<IInteractivity>().ToConstant(new ConsoleInteractivity());
            Bind<IFileSystem>().ToConstant(new RealFileSystem());
            Bind<ITutorialRunner>().To<TutorialRunner>();
            Bind<IFileReader>().To<OpenFileReader>();
            Bind<IResolver>().To<ReplResolver>();
        }
    }
}
