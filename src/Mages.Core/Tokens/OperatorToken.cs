namespace Mages.Core.Tokens
{
    using System;

    sealed class OperatorToken : IToken
    {
        private readonly TokenType _type;
        private readonly String _payload;
        private readonly TextPosition _start;
        private readonly TextPosition _end;

        public OperatorToken(TokenType type, String payload, TextPosition position)
            : this(type, payload, position, position)
        {
        }

        public OperatorToken(TokenType type, String payload, TextPosition start, TextPosition end)
        {
            _type = type;
            _payload = payload;
            _start = start;
            _end = end;
        }

        public TokenType Type => _type;

        public String Payload => _payload;

        public TextPosition Start => _start;

        public TextPosition End => _end;

        public override String ToString()
        {
            return $"Operator / {_start} -- {_end} / '{_payload}'";
        }
    }
}
