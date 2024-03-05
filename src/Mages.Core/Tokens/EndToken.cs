namespace Mages.Core.Tokens
{
    using System;

    sealed class EndToken(TextPosition position) : IToken
    {
        private readonly TextPosition _position = position;

        public TokenType Type => TokenType.End;

        public String Payload => String.Empty;

        public TextPosition Start => _position;

        public TextPosition End => _position;

        public override String ToString()
        {
            return $"EOF / {_position}";
        }
    }
}
