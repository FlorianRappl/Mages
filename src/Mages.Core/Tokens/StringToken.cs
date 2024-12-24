namespace Mages.Core.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;

sealed class StringToken(String content, IEnumerable<ParseError> errors, TextPosition start, TextPosition end) : IToken
{
    private static readonly IEnumerable<ParseError> NoErrors = [];

    private readonly String _content = content;
    private readonly TextPosition _start = start;
    private readonly TextPosition _end = end;
    private readonly IEnumerable<ParseError> _errors = errors ?? NoErrors;

    public IEnumerable<ParseError> Errors => _errors;

    public TokenType Type => TokenType.String;

    public String Payload => _content;

    public TextPosition Start => _start;

    public TextPosition End => _end;

    public override String ToString()
    {
        return $"String / {_start} -- {_end} / '{_content}'";
    }
}
