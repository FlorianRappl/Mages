namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents a while statement.
    /// </summary>
    public sealed class WhileStatement : BreakableStatement, IStatement
    {
        #region Fields

        private readonly IExpression _condition;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new while statement.
        /// </summary>
        /// <param name="condition">The condition to use.</param>
        /// <param name="body">The body to use.</param>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        public WhileStatement(IExpression condition, IStatement body, TextPosition start, TextPosition end)
            : base(body, start, end)
        {
            _condition = condition;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the stored condition.
        /// </summary>
        public IExpression Condition
        {
            get { return _condition; }
        }

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
