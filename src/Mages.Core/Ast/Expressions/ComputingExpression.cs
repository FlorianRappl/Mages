namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents a computed expression (not-assignable).
    /// </summary>
    abstract class ComputingExpression : BaseExpression
    {
        #region ctor

        public ComputingExpression(TextPosition start, TextPosition end)
            : base(start, end)
        {
        }

        #endregion

        #region Properties

        public Boolean IsAssignable
        {
            get { return false; }
        }

        #endregion
    }
}
