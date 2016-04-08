namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an empty expression (potentially invalid).
    /// </summary>
    sealed class EmptyExpression : ComputingExpression, IExpression
    {
        #region ctor

        public EmptyExpression(TextPosition position)
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
