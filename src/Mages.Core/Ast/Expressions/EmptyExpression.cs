namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an empty expression (potentially invalid).
    /// </summary>
    /// <remarks>
    /// Creates a new empty expression.
    /// </remarks>
    public sealed class EmptyExpression(TextPosition position) : ComputingExpression(position, position), IExpression
    {

        #region ctor

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
