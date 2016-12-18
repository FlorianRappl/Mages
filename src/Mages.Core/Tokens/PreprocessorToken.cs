namespace Mages.Core.Tokens
{
    using System;

    sealed class PreprocessorToken : IToken
    {
        private readonly TextPosition _start;
        private readonly TextPosition _end;
        private readonly String _payload;

        public PreprocessorToken(String payload, TextPosition start, TextPosition end)
        {
            _payload = payload;
            _start = start;
            _end = end;
        }

        public TokenType Type
        {
            get { return TokenType.Preprocessor; }
        }

        public TextPosition End
        {
            get { return _end; }
        }

        public TextPosition Start
        {
            get { return _start; }
        }

        public String Payload
        {
            get { return _payload; }
        }
    }
}