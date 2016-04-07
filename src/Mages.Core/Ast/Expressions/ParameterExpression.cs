namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an expression containing function parameters.
    /// </summary>
    sealed class ParameterExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression[] _expressions;

        #endregion

        #region ctor

        public ParameterExpression(IExpression[] expressions, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _expressions = expressions;
        }

        #endregion

        #region Properties

        public IExpression[] Expressions
        {
            get { return _expressions; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            foreach (var expression in _expressions)
            {
                if (expression is IdentifierExpression)
                {
                    _expressions.Validate(context);
                }
                else
                {
                    //TODO Report error
                }
            }
        }

        #endregion
    }
}
