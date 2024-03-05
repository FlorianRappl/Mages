namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents a while statement.
    /// </summary>
    /// <remarks>
    /// Creates a new while statement.
    /// </remarks>
    public sealed class WhileStatement(IExpression condition, IStatement body, TextPosition start) : BreakableStatement(body, start, body.End), IStatement
    {
        #region Fields

        private readonly IExpression _condition = condition;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the stored condition.
        /// </summary>
        public IExpression Condition => _condition;

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
