namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an object literal.
    /// </summary>
    /// <remarks>
    /// Creates a new object expression.
    /// </remarks>
    public sealed class ObjectExpression(IExpression[] values, TextPosition start, TextPosition end) : ComputingExpression(start, end), IExpression
    {
        #region Fields

        private readonly IExpression[] _values = values;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained expressions.
        /// </summary>
        public IExpression[] Values => _values;

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
