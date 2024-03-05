namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents the shared core of all expressions.
    /// </summary>
    /// <remarks>
    /// Creates a new expression.
    /// </remarks>
    public abstract class BaseExpression(TextPosition start, TextPosition end)
    {
        #region Fields

        private readonly TextPosition _start = start;
        private readonly TextPosition _end = end;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the start position of the expression.
        /// </summary>
        public TextPosition Start => _start;

        /// <summary>
        /// Gets the end position of the expression.
        /// </summary>
        public TextPosition End => _end;

        #endregion
    }
}
