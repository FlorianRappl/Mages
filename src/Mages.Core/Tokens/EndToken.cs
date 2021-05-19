﻿namespace Mages.Core.Tokens
{
    using System;

    sealed class EndToken : IToken
    {
        private readonly TextPosition _position;

        public EndToken(TextPosition position)
        {
            _position = position;
        }

        public TokenType Type => TokenType.End;

        public String Payload => String.Empty;

        public TextPosition Start => _position;

        public TextPosition End => _position;
    }
}
