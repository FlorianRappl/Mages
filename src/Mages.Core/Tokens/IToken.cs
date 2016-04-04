namespace Mages.Core.Tokens
{
    using System;

    /// <summary>
    /// Represents a token found by the tokenizer.
    /// </summary>
    public interface IToken
    {
        TokenType Type { get; }

        String Payload { get; }
    }
}
