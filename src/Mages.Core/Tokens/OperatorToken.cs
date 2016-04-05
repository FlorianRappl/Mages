namespace Mages.Core.Tokens
{
    using System;

    sealed class OperatorToken : IToken
    {
        private readonly TokenType _type;
        private readonly String _payload;
        private readonly ITextPosition _start;
        private readonly ITextPosition _end;

        public OperatorToken(TokenType type, String payload, ITextPosition position)
            : this(type, payload, position, position)
        {
        }

        public OperatorToken(TokenType type, String payload, ITextPosition start, ITextPosition end)
        {
            _type = type;
            _payload = payload;
            _start = start;
            _end = end;
        }

        public TokenType Type
        {
            get { return _type; }
        }

        public String Payload
        {
            get { return _payload; }
        }

        public ITextPosition Start
        {
            get { return _start; }
        }

        public ITextPosition End
        {
            get { return _end; }
        }
    }
}
