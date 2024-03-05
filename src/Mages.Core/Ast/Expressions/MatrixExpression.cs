namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a matrix expression.
    /// </summary>
    /// <remarks>
    /// Creates a new matrix expression.
    /// </remarks>
    public sealed class MatrixExpression(IExpression[][] values, TextPosition start, TextPosition end) : ComputingExpression(start, end), IExpression
    {
        #region Fields

        private readonly IExpression[][] _values = values;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the initialized values.
        /// </summary>
        public IExpression[][] Values => _values;

        #endregion

        #region Methods

        /// <summary>
        /// Accepts the visitor by showing him around.
        /// </summary>
        /// <param name="visitor">The visitor walking the tree.</param>
        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public void Validate(IValidationContext context)
        {
            var columns = _values.Length > 0 ? _values[0].Length : 0;

            foreach (var row in _values)
            {
                if (row.Length != columns)
                {
                    var error = new ParseError(ErrorCode.MatrixColumnsDiscrepency, this);
                    context.Report(error);
                    break;
                }
            }
        }

        #endregion
    }
}
