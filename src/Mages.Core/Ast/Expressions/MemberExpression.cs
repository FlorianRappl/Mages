namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a member expression.
    /// </summary>
    /// <remarks>
    /// Creates a new member expression.
    /// </remarks>
    public sealed class MemberExpression(IExpression obj, IExpression member) : AssignableExpression(obj.Start, member.End), IExpression
    {
        #region Fields

        private readonly IExpression _obj = obj;
        private readonly IExpression _member = member;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the associated object expression.
        /// </summary>
        public IExpression Object => _obj;

        /// <summary>
        /// Gets the associated member access.
        /// </summary>
        public IExpression Member => _member;

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
