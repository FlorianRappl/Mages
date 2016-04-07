namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a function call.
    /// </summary>
    sealed class CallExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression _function;
        private readonly ArgumentsExpression _arguments;

        #endregion

        #region ctor

        public CallExpression(IExpression function, ArgumentsExpression arguments)
            : base(function.Start, arguments.End)
        {
            _function = function;
            _arguments = arguments;
        }

        #endregion

        #region Properties

        public IExpression Function 
        {
            get { return _function; }
        }

        public ArgumentsExpression Arguments
        {
            get { return _arguments; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            _function.Validate(context);
            _arguments.Validate(context);
        }

        #endregion
    }
}
