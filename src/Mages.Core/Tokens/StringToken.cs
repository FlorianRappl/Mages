using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mages.Core.Tokens
{
    sealed class StringToken : IToken
    {
        private readonly String _content;
        private readonly ITextPosition _start;
        private readonly ITextPosition _end;

        public StringToken(String content, ITextPosition start, ITextPosition end)
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
