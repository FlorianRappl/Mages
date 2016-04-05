namespace Mages.Core.Ast
{
    using System;

    /// <summary>
    /// An abstract expression from the AST.
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// Gets the start position of the expression.
        /// </summary>
        TextPosition Start { get; }

        /// <summary>
        /// Gets the end position of the expression.
        /// </summary>
        TextPosition End { get; }

        /// <summary>
        /// Gets if the expression can be used as a value container.
        /// </summary>
        Boolean IsAssignable { get; }

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        void Validate(IValidationContext context);
    }
}
