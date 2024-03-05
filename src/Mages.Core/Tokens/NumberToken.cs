namespace Mages.Core.Tokens
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    sealed class NumberToken(Double value, IEnumerable<ParseError> errors, TextPosition start, TextPosition end) : IToken
    {
        private static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();

        private readonly Double _value = value;
        private readonly TextPosition _start = start;
        private readonly TextPosition _end = end;
        private readonly IEnumerable<ParseError> _errors = errors ?? NoErrors;

        public IEnumerable<ParseError> Errors => _errors;

        public Double Value => _value;

        public TokenType Type => TokenType.Number;

        public String Payload => _value.ToString(NumberFormatInfo.InvariantInfo);

        public TextPosition Start => _start;

        public TextPosition End => _end;

        public override String ToString()
        {
            return $"Number / {_start} -- {_end} / '{_value}'";
        }
    }
}
