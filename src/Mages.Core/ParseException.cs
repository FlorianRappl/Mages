namespace Mages.Core
{
    using System;

    /// <summary>
    /// Represents the exception that is thrown on trying
    /// to interpret invalid code.
    /// </summary>
    /// <remarks>
    /// Creates a new parse exception.
    /// </remarks>
    /// <param name="error">The error that occured.</param>
    public class ParseException(ParseError error) : Exception("The given source code contains errors.")
    {

        /// <summary>
        /// Gets the detected parse error.
        /// </summary>
        public ParseError Error
        {
            get;
            private set;
        } = error;
    }
}
