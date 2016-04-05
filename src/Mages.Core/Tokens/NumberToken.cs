namespace Mages.Core.Tokens
{
    using System;
    using System.Globalization;

    sealed class NumberToken : IToken
    {
        private readonly Double _value;
        private readonly TextPosition _start;
        private readonly TextPosition _end;

        public NumberToken(Double value, TextPosition start, TextPosition end)
        {
            _value = value;
            _start = start;
            _end = end;
        }

        public Double Value 
        {
            get { return _value; }
        }

        public TokenType Type
        {
            get { return TokenType.Number; }
        }

        public String Payload
        {
            get { return _value.ToString(NumberFormatInfo.InvariantInfo); }
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
