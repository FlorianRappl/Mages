namespace Mages.Core.Tokens
{
    using System;

    /// <summary>
    /// Represents a token found by the tokenizer.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        TokenType Type { get; }

        /// <summary>
        /// Gets the payload of the token.
        /// </summary>
        String Payload { get; }

        /// <summary>
        /// Gets the start position of the token.
        /// </summary>
        ITextPosition Start { get; }

        /// <summary>
        /// Gets the end position of the token.
        /// </summary>
        ITextPosition End { get; }
    }
}
