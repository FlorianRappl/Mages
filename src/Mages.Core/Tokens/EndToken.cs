namespace Mages.Core.Tokens
{
    using System;

    sealed class EndToken : IToken
    {
        private readonly ITextPosition _position;

        public EndToken(ITextPosition position)
        {
            _position = position;
        }

        public TokenType Type
        {
            get { return TokenType.End; }
        }

        public String Payload
        {
            get { return String.Empty; }
        }

        public ITextPosition Start
        {
            get { return _position; }
        }

        public ITextPosition End
        {
            get { return _position; }
        }
    }
}
