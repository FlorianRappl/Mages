namespace Mages.Repl
{
    using Ninject.Modules;

    sealed class ReplServices : NinjectModule
    {
        public override void Load()
        {
            Bind<IInteractivity>().ToConstant(new ConsoleInteractivity());
            Bind<IFileReader>().ToConstant(new ModuleFileReader());
            Bind<IMagesCreator>().ToConstant(new MagesCreator());
        }
    }
}
