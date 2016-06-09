namespace Mages.Repl.Plugins
{
    using Mages.Core;
    using Mages.Modules.LinearAlgebra;

    static class ReplPlugins
    {
        public static void Integrate(Engine engine)
        {
            engine.AddPlugin(typeof(LinearAlgebraPlugin));
        }
    }
}
