namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an expression containing function parameters.
    /// </summary>
    public sealed class ParameterExpression : ComputingExpression, IExpression
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

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void BindTo(AbstractScope scope)
        {
            for (var i = 0; i < _expressions.Length; i++)
            {
                var variable = _expressions[i] as VariableExpression;

                if (variable != null)
                {
                    var expression = new IdentifierExpression(variable.Name, variable.Start, variable.End);
                    _expressions[i] = expression;
                    scope.Provide(variable.Name, expression);
                }
            }
        }

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
