namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a function expression.
    /// </summary>
    sealed class FunctionExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly AbstractScope _scope;
        private readonly ParameterExpression _parameters;
        private readonly IExpression _body;

        #endregion

        #region ctor

        public FunctionExpression(AbstractScope scope, ParameterExpression parameters, IExpression body)
            : base(parameters.Start, parameters.End)
        {
            _scope = scope;
            _parameters = parameters;
            _body = body;
        }

        #endregion

        #region Properties

        public AbstractScope Scope
        {
            get { return _scope; }
        }

        public ParameterExpression Parameters
        {
            get { return _parameters; }
        }

        public IExpression Body
        {
            get { return _body; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            _parameters.Validate(context);
            _body.Validate(context);
        }

        #endregion
    }
}
