namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents a computed expression (not-assignable).
    /// </summary>
    /// <remarks>
    /// Creates a new computing expression.
    /// </remarks>
    public abstract class ComputingExpression(TextPosition start, TextPosition end) : BaseExpression(start, end)
    {

        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the expression is assignable.
        /// </summary>
        public Boolean IsAssignable => false;

        #endregion
    }
}
