namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an expression containing function parameters.
    /// </summary>
    public sealed class ParameterExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression[] _expressions;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new parameter expression.
        /// </summary>
        public ParameterExpression(IExpression[] expressions, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _expressions = expressions;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained expressions.
        /// </summary>
        public IExpression[] Expressions
        {
            get { return _expressions; }
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
            foreach (var expression in _expressions)
            {
                if (expression is VariableExpression == false)
                {
                    var error = new ParseError(ErrorCode.IdentifierExpected, expression);
                    context.Report(error);
                }
            }
        }

        #endregion
    }
}
