namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an empty expression (illegal).
    /// </summary>
    sealed class EmptyExpression : ComputingExpression, IExpression
    {
        #region ctor

        public EmptyExpression(ITextPosition position)
            : base(position, position)
        {
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
        }

        #endregion
    }
}
