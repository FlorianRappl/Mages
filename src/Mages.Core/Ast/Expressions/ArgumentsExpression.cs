namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// The class for an argument expression.
    /// </summary>
    /// <remarks>
    /// Creates new arguments expression.
    /// </remarks>
    public sealed class ArgumentsExpression(IExpression[] arguments, TextPosition start, TextPosition end) : ComputingExpression(start, end), IExpression
    {
        #region Fields

        private readonly IExpression[] _arguments = arguments;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the stored arguments.
        /// </summary>
        public IExpression[] Arguments => _arguments;

        /// <summary>
        /// Gets the number of arguments.
        /// </summary>
        public Int32 Count => _arguments.Length;

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
