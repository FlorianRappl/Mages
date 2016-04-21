namespace Mages.Core
{
    using System;

    public static class EngineExtensions
    {
        public static void Evaluate(this Engine engine, String code)
        {
            var parser = engine.Parser;
            var statement = parser.ParseStatement(code);

            //TODO
        }
    }
}
