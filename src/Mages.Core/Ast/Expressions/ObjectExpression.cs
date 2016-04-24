namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an object literal.
    /// </summary>
    public sealed class ObjectExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression[] _values;

        #endregion

        #region ctor

        public ObjectExpression(IExpression[] values, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _values = values;
        }

        #endregion

        #region Properties

        public IExpression[] Values
        {
            get { return _values; }
        }

        #endregion

        #region Methods

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            foreach (var value in _values)
            {
                value.Validate(context);
            }
        }

        #endregion
    }
}
