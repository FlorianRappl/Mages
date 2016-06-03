namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents an if statement.
    /// </summary>
    public sealed class IfStatement : BaseStatement, IStatement
    {
        #region Fields

        private readonly IExpression _condition;
        private readonly IStatement _primary;
        private readonly IStatement _secondary;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new if statement.
        /// </summary>
        /// <param name="condition">The condition to use.</param>
        /// <param name="primary">The primary body to use.</param>
        /// <param name="secondary">The secondary body to use.</param>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        public IfStatement(IExpression condition, IStatement primary, IStatement secondary, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _condition = condition;
            _primary = primary;
            _secondary = secondary;
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
        /// Gets the primary body.
        /// </summary>
        public IStatement Primary
        {
            get { return _primary; }
        }

        /// <summary>
        /// Gets the secondary body.
        /// </summary>
        public IStatement Secondary
        {
            get { return _secondary; }
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
            if (_primary.IsEmpty())
            {
                var error = new ParseError(ErrorCode.ExpressionExpected, _primary);
                context.Report(error);
            }
        }

        #endregion
    }
}
