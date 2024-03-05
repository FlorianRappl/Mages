namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents an expression that can be assigned.
    /// </summary>
    /// <remarks>
    /// Creates a new assignable expression.
    /// </remarks>
    public abstract class AssignableExpression(TextPosition start, TextPosition end) : BaseExpression(start, end)
    {

        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the expression is assignable.
        /// </summary>
        public Boolean IsAssignable => true;

        #endregion
    }
}
