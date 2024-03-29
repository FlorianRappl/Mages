﻿namespace Mages.Core.Tokens;

using Mages.Core.Source;
using System;

sealed class PreprocessorToken(String payload, TextPosition start, TextPosition end) : IToken
{
    private readonly TextPosition _start = start;
    private readonly TextPosition _end = end;
    private readonly String _payload = payload;

    public TokenType Type => TokenType.Preprocessor;

    public TextPosition End => _end;

    public TextPosition Start => _start;

    public String Command
    {
        get
        {
            if (_payload.Length > 0 && Specification.IsNameStart((Int32)_payload[0]))
            {
                var length = 1;

                while (length < _payload.Length && Specification.IsName((Int32)_payload[length]))
                {
                    length++;
                }

                return _payload.Substring(0, length);
            }

            return String.Empty;
        }
    }

    public String Payload => _payload;
}