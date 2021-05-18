namespace Mages.Plugins.Transpilers
{
    using Mages.Core;
    using Mages.Plugins.Transpilers.TreeWalkers;
    using System;

    public sealed class Transpiler
    {
        private readonly Engine _engine;

        public Transpiler(Engine engine)
        {
            _engine = engine;
        }

        public String Js(String content)
        {
            var walker = new JavaScriptTreeWalker();
            return Transform(walker, content);
        }

        private String Transform(TranspilerTreeWalker walker, String content)
        {
            var parser = _engine.Parser;
            var source = content.ToTokenStream();
            var statements = parser.ParseStatements(source);
            return walker.Transform(statements);
        }
    }
}