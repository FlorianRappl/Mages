namespace Mages.Core.Tokens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class StringToken : IToken
    {
        private static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();

        private readonly String _content;
        private readonly TextPosition _start;
        private readonly TextPosition _end;
        private readonly IEnumerable<ParseError> _errors;

        public StringToken(String content, IEnumerable<ParseError> errors, TextPosition start, TextPosition end)
        {
            _content = content;
            _start = start;
            _end = end;
            _errors = errors ?? NoErrors;
        }

        public IEnumerable<ParseError> Errors => _errors;

        public TokenType Type => TokenType.String;

        public String Payload => _content;

        public TextPosition Start => _start;

        public TextPosition End => _end;
    }
}
