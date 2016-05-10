namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents a simple statement containing an expression.
    /// </summary>
    public sealed class SimpleStatement : BaseStatement, IStatement
    {
        #region Fields

        private readonly IExpression _expression;

        #endregion

        #region ctor

        public SimpleStatement(IExpression expression, TextPosition end)
            : base(expression.Start, end)
        {
            _expression = expression;
        }

        #endregion

        #region Properties

        public IExpression Expression
        {
            get { return _expression; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            _expression.Validate(context);
        }

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        #endregion
    }
}
