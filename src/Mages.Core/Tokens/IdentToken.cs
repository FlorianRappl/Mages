namespace Mages.Core.Tokens
{
    using System;

    sealed class IdentToken(TokenType type, String identifier, TextPosition start, TextPosition end) : IToken
    {
        private readonly TokenType _type = type;
        private readonly String _identifier = identifier;
        private readonly TextPosition _start = start;
        private readonly TextPosition _end = end;

        public TokenType Type => _type;

        public String Payload => _identifier;

        public TextPosition Start => _start;

        public TextPosition End => _end;

        public override String ToString()
        {
            return $"Identifier / {_start} -- {_end} / '{_identifier}'";
        }
    }
}
