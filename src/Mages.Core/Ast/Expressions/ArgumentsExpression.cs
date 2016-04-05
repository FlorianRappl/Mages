namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// The class for an argument expression.
    /// </summary>
    sealed class ArgumentsExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression[] _expressions;

        #endregion

        #region ctor

        public ArgumentsExpression(IExpression[] expressions, TextPosition start, TextPosition end)
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
            _expressions.Validate(context);
        }

        #endregion
    }
}
