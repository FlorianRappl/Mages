namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an invalid expression.
    /// </summary>
    public sealed class InvalidExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly ErrorCode _error;
        private readonly ITextRange _payload;

        #endregion

        #region ctor

        public InvalidExpression(ErrorCode error, ITextRange payload)
            : base(payload.Start, payload.End)
        {
            _error = error;
            _payload = payload;
        }

        #endregion

        #region Properties

        public ITextRange Payload
        {
            get { return _payload; }
        }

        public ErrorCode Error
        {
            get { return _error; }
        }

        #endregion

        #region Methods

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            context.Report(new ParseError(_error, _payload));
        }

        #endregion
    }
}
