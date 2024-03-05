namespace Mages.Core.Ast.Statements
{
    using System;

    /// <summary>
    /// Represents a for statement.
    /// </summary>
    /// <remarks>
    /// Creates a new for statement.
    /// </remarks>
    public sealed class ForStatement(Boolean declared, IExpression initialization, IExpression condition, IExpression afterthought, IStatement body, TextPosition start) : BreakableStatement(body, start, body.End), IStatement
    {
        #region Fields

        private readonly Boolean _declared = declared;
        private readonly IExpression _initialization = initialization;
        private readonly IExpression _condition = condition;
        private readonly IExpression _afterthought = afterthought;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the initialization variable is declared.
        /// </summary>
        public Boolean IsDeclared => _declared;

        /// <summary>
        /// Gets the stored initialization.
        /// </summary>
        public IExpression Initialization => _initialization;

        /// <summary>
        /// Gets the stored condition.
        /// </summary>
        public IExpression Condition => _condition;

        /// <summary>
        /// Gets the stored after thought.
        /// </summary>
        public IExpression AfterThought => _afterthought;

        #endregion

        #region Methods

        /// <summary>
        /// Accepts the visitor by showing him around.
        /// </summary>
        /// <param name="visitor">The visitor walking the tree.</param>
        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        #endregion
    }
}
