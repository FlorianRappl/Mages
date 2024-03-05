namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents an interpolated string expression.
    /// </summary>
    /// <remarks>
    /// Creates a new interpolated string expression.
    /// </remarks>
    public sealed class InterpolatedExpression(ConstantExpression format, IExpression[] replacements) : ComputingExpression(format.Start, format.End), IExpression
    {
        #region Fields

        private readonly ConstantExpression _format = format;
        private readonly IExpression[] _replacements = replacements;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the formatting string.
        /// </summary>
        public ConstantExpression Format => _format;

        /// <summary>
        /// Gets the associated replacements.
        /// </summary>
        public IExpression[] Replacements => _replacements;

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
