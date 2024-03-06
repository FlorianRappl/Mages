namespace Mages.Core.Tokens;

using System;

sealed class OperatorToken(TokenType type, String payload, TextPosition start, TextPosition end) : IToken
{
    private readonly TokenType _type = type;
    private readonly String _payload = payload;
    private readonly TextPosition _start = start;
    private readonly TextPosition _end = end;

    public OperatorToken(TokenType type, String payload, TextPosition position)
        : this(type, payload, position, position)
    {
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
