namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents a generalized identifier, which is not a variable.
    /// </summary>
    /// <remarks>
    /// Creates a new identifier expression.
    /// </remarks>
    public sealed class IdentifierExpression(String name, TextPosition start, TextPosition end) : ComputingExpression(start, end), IExpression
    {
        #region Fields

        private readonly String _name = name;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the identifier.
        /// </summary>
        public String Name => _name;

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
