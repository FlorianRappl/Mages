namespace Mages.Core.Ast.Expressions
{
    using System;

    abstract class AssignableExpression : BaseExpression
    {
        #region ctor

        public AssignableExpression(TextPosition start, TextPosition end)
            : base(start, end)
        {
        }

        #endregion

        #region Properties

        public Boolean IsAssignable
        {
            get { return true; }
        }

        #endregion
    }
}
