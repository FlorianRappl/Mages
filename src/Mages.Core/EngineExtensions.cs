namespace Mages.Core
{
    using System;

    /// <summary>
    /// A collection of useful extensions for the engine.
    /// </summary>
    public static class EngineExtensions
    {
        /// <summary>
        /// Evaluates the given code.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="code">The code to evaluate.</param>
        public static void Evaluate(this Engine engine, String code)
        {
            var parser = engine.Parser;
            var statement = parser.ParseStatement(code);

            //TODO
        }
    }
}
