namespace Mages.Core
{
    using Mages.Core.Ast;

    /// <summary>
    /// Represents the central engine for any kind of evaluation.
    /// </summary>
    public class Engine
    {
        private readonly IParser _parser;

        /// <summary>
        /// Creates a new engine with the specified elements. Otherwise default
        /// elements are being used.
        /// </summary>
        /// <param name="parser">The parser to use.</param>
        public Engine(IParser parser = null)
        {
            _parser = parser ?? new ExpressionParser();
        }

        /// <summary>
        /// Gets the used parser instance.
        /// </summary>
        public IParser Parser
        {
            get { return _parser; }
        }

        public void Evaluate(IStatement statement)
        {

        }
    }
}
