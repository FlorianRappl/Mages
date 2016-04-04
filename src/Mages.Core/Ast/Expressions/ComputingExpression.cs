namespace Mages.Core.Ast.Expressions
{
    using System;

    abstract class ComputingExpression : BaseExpression
    {
        #region ctor

        public ComputingExpression(ITextPosition start, ITextPosition end)
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
