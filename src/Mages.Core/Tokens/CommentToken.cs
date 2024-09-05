namespace Mages.Core.Tokens;

using System;

sealed class CommentToken(String comment, TextPosition start, TextPosition end) : IToken
{
    private readonly String _comment = comment;
    private readonly TextPosition _start = start;
    private readonly TextPosition _end = end;

    public TokenType Type => TokenType.Comment;

    public String Payload => _comment;

    public TextPosition Start => _start;

    public TextPosition End => _end;

    public override String ToString()
    {
        return $"Comment / {_start} -- {_end} / '{_comment}'";
    }
}
