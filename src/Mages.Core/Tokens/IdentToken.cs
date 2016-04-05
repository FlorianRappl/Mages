namespace Mages.Core.Tokens
{
    using System;

    sealed class IdentToken : IToken
    {
        private readonly TokenType _type;
        private readonly String _identifier;
        private readonly ITextPosition _start;
        private readonly ITextPosition _end;

        public IdentToken(TokenType type, String identifier, ITextPosition start, ITextPosition end)
        {
            _type = type;
            _identifier = identifier;
            _start = start;
            _end = end;
        }

        public TokenType Type
        {
            get { return _type; }
        }

        public String Payload
        {
            get { return _identifier; }
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
