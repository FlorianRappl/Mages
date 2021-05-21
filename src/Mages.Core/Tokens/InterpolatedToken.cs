namespace Mages.Core.Tokens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class InterpolatedToken : IToken
    {
        private static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();

        private readonly String _content;
        private readonly TextPosition _start;
        private readonly TextPosition _end;
        private readonly IEnumerable<ParseError> _errors;
        private readonly List<List<IToken>> _parts;

        public InterpolatedToken(String content, List<List<IToken>> parts, IEnumerable<ParseError> errors, TextPosition start, TextPosition end)
        {
            _content = content;
            _start = start;
            _end = end;
            _errors = errors ?? NoErrors;
            _parts = parts;
        }

        public Int32 ReplacementCount => _parts.Count;

        public IEnumerable<IToken> this[Int32 index]
        {
            get { return _parts[index]; }
        }

        public IEnumerable<ParseError> Errors => _errors;

        public TokenType Type => TokenType.InterpolatedString;

        public String Payload => _content;

        public TextPosition Start => _start;

        public TextPosition End => _end;

        public override String ToString()
        {
            return $"InterpolatedString / {_start} -- {_end} / '{_content}'";
        }
    }
}
