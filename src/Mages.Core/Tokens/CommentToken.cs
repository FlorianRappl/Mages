namespace Mages.Core.Tokens
{
    using System;

    sealed class CommentToken : IToken
    {
        private readonly TokenType _type;
        private readonly String _comment;
        private readonly ITextPosition _start;
        private readonly ITextPosition _end;

        public CommentToken(TokenType type, String comment, ITextPosition start, ITextPosition end)
        {
            _type = type;
            _comment = comment;
            _start = start;
            _end = end;
        }

        public TokenType Type
        {
            get { return _type; }
        }

        public String Payload
        {
            get { return _comment; }
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
