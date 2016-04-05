namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a function expression.
    /// </summary>
    sealed class FunctionExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly AbstractScope _scope;
        private readonly VariableExpression[] _parameters;

        #endregion

        #region ctor

        public FunctionExpression(AbstractScope scope, VariableExpression[] parameters, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _scope = scope;
            _parameters = parameters;
        }

        #endregion

        #region Properties

        public AbstractScope Scope
        {
            get { return _scope; }
        }

        public VariableExpression[] Parameters
        {
            get { return _parameters; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
        }

        #endregion
    }
}
