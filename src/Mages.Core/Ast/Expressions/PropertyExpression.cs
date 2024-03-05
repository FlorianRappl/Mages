namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a property (name-value pair) of an object.
    /// </summary>
    /// <remarks>
    /// Creates a new property.
    /// </remarks>
    public sealed class PropertyExpression(IExpression name, IExpression value) : ComputingExpression(name.Start, value.End), IExpression
    {
        #region Fields

        private readonly IExpression _name = name;
        private readonly IExpression _value = value;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public IExpression Name => _name;

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        public IExpression Value => _value;

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
