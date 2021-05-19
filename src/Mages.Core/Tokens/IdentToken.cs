namespace Mages.Core.Tokens
{
    using System;

    sealed class IdentToken : IToken
    {
        private readonly TokenType _type;
        private readonly String _identifier;
        private readonly TextPosition _start;
        private readonly TextPosition _end;

        public IdentToken(TokenType type, String identifier, TextPosition start, TextPosition end)
        {
            _type = type;
            _identifier = identifier;
            _start = start;
            _end = end;
        }

        public TokenType Type => _type;

        public String Payload => _identifier;

        public TextPosition Start => _start;

        public TextPosition End => _end;
    }
}
