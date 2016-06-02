namespace Mages.Core.Ast.Statements
{
    using Mages.Core.Ast.Expressions;

    /// <summary>
    /// Represents a while statement.
    /// </summary>
    public sealed class WhileStatement : BaseStatement, IStatement
    {
        #region Fields

        private readonly IExpression _condition;
        private readonly IStatement _body;

        #endregion

        #region

        /// <summary>
        /// Creates a new while statement.
        /// </summary>
        /// <param name="condition">The condition to use.</param>
        /// <param name="body">The body to use.</param>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        public WhileStatement(IExpression condition, IStatement body, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _condition = condition;
            _body = body;
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
        /// Gets the stored body.
        /// </summary>
        public IStatement Body
        {
            get { return _body; }
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
            if (_body.IsEmpty())
            {
                var error = new ParseError(ErrorCode.ExpressionExpected, _body);
                context.Report(error);
            }
        }

        #endregion
    }
}
