namespace Mages.Repl.Plugins
{
    using Mages.Core;
    using Mages.Modules.Draw;
    using Mages.Modules.FileSystem;
    using Mages.Modules.LinearAlgebra;

    static class ReplPlugins
    {
        public static void Integrate(Engine engine)
        {
            engine.AddPlugin(typeof(LinearAlgebraPlugin));
            engine.AddPlugin(typeof(FileSystemPlugin));
            engine.AddPlugin(typeof(DrawPlugin));
        }
    }
}
