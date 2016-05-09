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
        private readonly IDictionary<String, IMagesType> _scope;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new engine with the specified elements. Otherwise default
        /// elements are being used.
        /// </summary>
        /// <param name="parser">The parser to use.</param>
        /// <param name="scope">The context to use.</param>
        public Engine(IParser parser = null, IDictionary<String, IMagesType> scope = null)
        {
            _parser = parser ?? new ExpressionParser();
            _scope = scope ?? new Dictionary<String, IMagesType>();
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
        public IDictionary<String, IMagesType> Scope
        {
            get { return _scope; }
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
            var operations = statements.MakeRunnable();
            operations.Execute(_scope);
            return operations.Pop();
        }

        #endregion
    }
}
