namespace Mages.Repl.Plugins
{
    using Mages.Core;
    using Mages.Plugins.Draw;
    using Mages.Plugins.FileSystem;
    using Mages.Plugins.LinearAlgebra;
    using Mages.Plugins.Plots;
    using Mages.Plugins.Random;
    using Mages.Plugins.Transpilers;

    static class ReplPlugins
    {
        public static void Integrate(Engine engine)
        {
            engine.AddPlugin(typeof(LinearAlgebraPlugin));
            engine.AddPlugin(typeof(FileSystemPlugin));
            engine.AddPlugin(typeof(DrawPlugin));
            engine.AddPlugin(typeof(PlotsPlugin));
            engine.AddPlugin(typeof(RandomPlugin));
            engine.AddPlugin(typeof(TranspilersPlugin));
        }
    }
}
