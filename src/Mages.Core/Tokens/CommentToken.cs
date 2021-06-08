namespace Mages.Core.Tokens
{
    using System;

    sealed class CommentToken : IToken
    {
        private readonly String _comment;
        private readonly TextPosition _start;
        private readonly TextPosition _end;

        public CommentToken(String comment, TextPosition start, TextPosition end)
        {
            _comment = comment;
            _start = start;
            _end = end;
        }

        public TokenType Type => TokenType.Comment;

        public String Payload => _comment;

        public TextPosition Start => _start;

        public TextPosition End => _end;

        public override String ToString()
        {
            return $"Comment / {_start} -- {_end} / '{_comment}'";
        }
    }
}
