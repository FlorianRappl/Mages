namespace Mages.Core.Tokens
{
    /// <summary>
    /// Represents the tokenizer performing the lexical analysis.
    /// </summary>
    public interface ITokenizer
    {
        /// <summary>
        /// Gets the next token from the scanner.
        /// </summary>
        /// <returns>The token.</returns>
        IToken Next();

        /// <summary>
        /// Gets the previous token from the scanner
        /// </summary>
        /// <returns>The token.</returns>
        IToken Previous();

        /// <summary>
        /// Gets the current token.
        /// </summary>
        IToken Current { get; }
    }
}
