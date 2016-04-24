namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a matrix expression.
    /// </summary>
    public sealed class MatrixExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression[][] _values;

        #endregion

        #region ctor

        public MatrixExpression(IExpression[][] values, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _values = values;
        }

        #endregion

        #region Properties

        public IExpression[][] Values
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
            var columns = _values.Length > 0 ? _values[0].Length : 0;

            foreach (var row in _values)
            {
                if (row.Length != columns)
                {
                    //TODO column mismatch error
                }

                row.Validate(context);
            }
        }

        #endregion
    }
}
