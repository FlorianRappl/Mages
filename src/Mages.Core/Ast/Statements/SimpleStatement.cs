namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents a simple statement containing an expression.
    /// </summary>
    /// <remarks>
    /// Creates a new simple statement.
    /// </remarks>
    public sealed class SimpleStatement(IExpression expression, TextPosition end) : BaseStatement(expression.Start, end), IStatement
    {
        #region Fields

        private readonly IExpression _expression = expression;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained expression.
        /// </summary>
        public IExpression Expression => _expression;

        #endregion

        #region Methods

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public void Validate(IValidationContext context)
        {
        }

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
