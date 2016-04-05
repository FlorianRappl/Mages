using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mages.Core.Tokens
{
    sealed class StringToken : IToken
    {
        private readonly String _content;
        private readonly TextPosition _start;
        private readonly TextPosition _end;

        public StringToken(String content, TextPosition start, TextPosition end)
        {
            _content = content;
            _start = start;
            _end = end;
        }

        public TokenType Type
        {
            get { return TokenType.Text; }
        }

        public String Payload
        {
            get { return _content; }
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
