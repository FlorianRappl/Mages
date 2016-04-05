namespace Mages.Core.Tokens
{
    using System;
    using System.Globalization;

    sealed class NumberToken : IToken
    {
        private readonly Double _value;
        private readonly ITextPosition _start;
        private readonly ITextPosition _end;

        public NumberToken(Double value, ITextPosition start, ITextPosition end)
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
