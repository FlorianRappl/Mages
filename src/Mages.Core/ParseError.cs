namespace Mages.Core
{
    /// <summary>
    /// A class to encapsulate data of a parse error.
    /// </summary>
    public sealed class ParseError
    {
        #region Fields

        private readonly ErrorCode _code;
        private readonly ITextPosition _position;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new parse error object with these properties.
        /// </summary>
        /// <param name="code">The code of the error.</param>
        /// <param name="position">The position of the error.</param>
        public ParseError(ErrorCode code, ITextPosition position)
        {
            _code = code;
            _position = position;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the position of the error.
        /// </summary>
        public ITextPosition Position
        {
            get { return _position; }
        }

        /// <summary>
        /// Gets the code of the error.
        /// </summary>
        public ErrorCode Code
        {
            get { return _code; }
        }

        #endregion
    }
}
