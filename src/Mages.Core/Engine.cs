namespace Mages.Core
{
    using Mages.Core.Ast;
    using Mages.Core.Vm;
    using System;

    /// <summary>
    /// Represents the central engine for any kind of evaluation.
    /// </summary>
    public class Engine
    {
        #region Fields

        private readonly IParser _parser;
        private readonly IMemory _memory;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new engine with the specified elements. Otherwise default
        /// elements are being used.
        /// </summary>
        /// <param name="parser">The parser to use.</param>
        /// <param name="memory">The memory to use.</param>
        public Engine(IParser parser = null, IMemory memory = null)
        {
            _parser = parser ?? new ExpressionParser();
            _memory = memory ?? new SimpleMemory();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the used parser instance.
        /// </summary>
        public IParser Parser
        {
            get { return _parser; }
        }

        /// <summary>
        /// Gets the used memory.
        /// </summary>
        public IMemory Memory
        {
            get { return _memory; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Interprets the given source and returns the result, if any.
        /// </summary>
        /// <param name="source">The source to interpret.</param>
        /// <returns>The result if available, otherwise null.</returns>
        public Object Interpret(String source)
        {
            var statements = _parser.ParseStatements(source);
            var operations = statements.MakeRunnable(_memory);
            operations.Execute();
            return operations.Pop();
        }

        #endregion
    }
}
