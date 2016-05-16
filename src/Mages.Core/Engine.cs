namespace Mages.Core
{
    using Mages.Core.Ast;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the central engine for any kind of evaluation.
    /// </summary>
    public class Engine
    {
        #region Fields

        private readonly IParser _parser;
        private readonly IDictionary<String, Object> _scope;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new engine with the specified elements. Otherwise default
        /// elements are being used.
        /// </summary>
        /// <param name="parser">The parser to use.</param>
        /// <param name="scope">The context to use.</param>
        public Engine(IParser parser = null, IDictionary<String, Object> scope = null)
        {
            _parser = parser ?? new ExpressionParser();
            _scope = scope ?? new Dictionary<String, Object>();
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
        /// Gets the used global scope.
        /// </summary>
        public IDictionary<String, Object> Scope
        {
            get { return _scope; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compiles the given source and returns a function to execute later.
        /// </summary>
        /// <param name="source">The source to compile.</param>
        /// <returns>The function to invoke later.</returns>
        public Func<Object> Compile(String source)
        {
            var statements = _parser.ParseStatements(source);
            var operations = statements.MakeRunnable();
            return () =>
            {
                operations.Execute(_scope);
                return operations.Pop();
            };
        }

        /// <summary>
        /// Interprets the given source and returns the result, if any.
        /// </summary>
        /// <param name="source">The source to interpret.</param>
        /// <returns>The result if available, otherwise null.</returns>
        public Object Interpret(String source)
        {
            var statements = _parser.ParseStatements(source);
            var operations = statements.MakeRunnable();
            operations.Execute(_scope);
            return operations.Pop();
        }

        #endregion
    }
}
