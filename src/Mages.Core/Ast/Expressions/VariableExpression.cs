namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents the access of a variable.
    /// </summary>
    /// <remarks>
    /// Creates a new variable expression.
    /// </remarks>
    public sealed class VariableExpression(String name, AbstractScope scope, TextPosition start, TextPosition end) : AssignableExpression(start, end), IExpression
    {
        #region Fields

        private readonly String _name = name;
        private readonly AbstractScope _scope = scope;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public String Name => _name;

        /// <summary>
        /// Gets the assigned abstract scope.
        /// </summary>
        public AbstractScope Scope => _scope;

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
