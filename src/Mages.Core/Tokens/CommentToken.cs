namespace Mages.Core.Tokens
{
    using System;

    sealed class CommentToken : IToken
    {
        private readonly TokenType _type;
        private readonly String _comment;
        private readonly TextPosition _start;
        private readonly TextPosition _end;

        public CommentToken(TokenType type, String comment, TextPosition start, TextPosition end)
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

        public TextPosition Start
        {
            get { return _start; }
        }

        public TextPosition End
        {
            get { return _end; }
        }
    }
}
