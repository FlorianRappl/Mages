namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents the shared core of all statements.
    /// </summary>
    /// <remarks>
    /// Creates a new statement.
    /// </remarks>
    public abstract class BaseStatement(TextPosition start, TextPosition end)
    {
        #region Fields

        private readonly TextPosition _start = start;
        private readonly TextPosition _end = end;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the start position of the statement.
        /// </summary>
        public TextPosition Start => _start;

        /// <summary>
        /// Gets the end position of the statement.
        /// </summary>
        public TextPosition End => _end;

        #endregion
    }
}
