namespace Mages.Plugins.Transpilers
{
    using Mages.Core;
    using System;

    public class TranspilersPlugin
    {
        public static readonly String Name = "Transpilers";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";
        private readonly Transpiler _transpiler;

        public TranspilersPlugin(Engine engine)
        {
            _transpiler = new Transpiler(engine);
        }

        public Transpiler TranspileTo
        {
            get { return _transpiler; }
        }
    }
}
