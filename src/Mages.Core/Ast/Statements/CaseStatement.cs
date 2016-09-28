namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents an case statement.
    /// </summary>
    public sealed class CaseStatement : BaseStatement, IStatement
    {
        #region Fields

        private readonly IExpression _condition;
        private readonly IStatement _block;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new case statement.
        /// </summary>
        public CaseStatement(IExpression condition, IStatement block)
            : base(condition.Start, block.End)
        {
            _condition = condition;
            _block = block;
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

        /// <summary>
        /// Gets the statement block.
        /// </summary>
        public IStatement Block
        {
            get { return _block; }
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

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public void Validate(IValidationContext context)
        {
        }

        #endregion
    }
}
