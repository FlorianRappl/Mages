namespace Mages.Core
{
    /// <summary>
    /// A class to encapsulate data of a parse error.
    /// </summary>
    /// <remarks>
    /// Creates a new parse error object with these properties.
    /// </remarks>
    /// <param name="code">The code of the error.</param>
    /// <param name="range">The text range of the error.</param>
    public sealed class ParseError(ErrorCode code, ITextRange range) : ITextRange
    {
        #region Fields

        private readonly ErrorCode _code = code;
        private readonly ITextRange _range = range;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the start position of the error.
        /// </summary>
        public TextPosition Start => _range.Start;

        /// <summary>
        /// Gets the end position of the error.
        /// </summary>
        public TextPosition End => _range.End;

        /// <summary>
        /// Gets the code of the error.
        /// </summary>
        public ErrorCode Code => _code;

        #endregion
    }
}
