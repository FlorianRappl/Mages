namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a function expression.
    /// </summary>
    public sealed class FunctionExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly ParameterExpression _parameters;
        private readonly IExpression _body;

        #endregion

        #region ctor

        public FunctionExpression(ParameterExpression parameters, IExpression body)
            : base(parameters.Start, parameters.End)
        {
            _parameters = parameters;
            _body = body;
        }

        #endregion

        #region Properties

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

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            _parameters.Validate(context);
            _body.Validate(context);
        }

        #endregion
    }
}
