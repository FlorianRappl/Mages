namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a function call.
    /// </summary>
    /// <remarks>
    /// Creates a new function call expression.
    /// </remarks>
    public sealed class CallExpression(IExpression function, ArgumentsExpression arguments) : AssignableExpression(function.Start, arguments.End), IExpression
    {
        #region Fields

        private readonly IExpression _function = function;
        private readonly ArgumentsExpression _arguments = arguments;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the associated function.
        /// </summary>
        public IExpression Function => _function;

        /// <summary>
        /// Gets the arguments to pass to the function.
        /// </summary>
        public ArgumentsExpression Arguments => _arguments;

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
