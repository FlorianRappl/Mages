﻿namespace Mages.Core.Tokens;

using Mages.Core.Source;
using System;
using System.Collections.Generic;

sealed class NumberTokenizer : ITokenizer
{
    #region Methods

    public IToken Next(IScanner scanner)
    {
        var state = new NumberState(scanner);
        var current = scanner.Current;

        if (current == CharacterTable.FullStop)
        {
            return state.Dot();
        }
        else if (current == CharacterTable.Zero)
        {
            return state.Zero();
        }

        return state.Digit();
    }

    #endregion

    #region Helpers

    private static Boolean IsDotOperator(Int32 current)
    {
        return current == CharacterTable.SingleQuotationMark ||
            current == CharacterTable.CircumflexAccent || current == CharacterTable.Asterisk ||
            current == CharacterTable.Slash || current == CharacterTable.Backslash;
    }

    #endregion

    #region State

    struct NumberState(IScanner scanner)
    {
        private readonly IScanner _scanner = scanner;
        private readonly TextPosition _start = scanner.Position;

        private List<ParseError> _errors = null;
        private UInt64 _value = 0;
        private UInt16 _digits = 0;
        private Int32 _powers = 0;
        private Int32 _shifts = 0;

        public Double Number => _value * Math.Pow(10.0, _shifts + _powers - _digits);

        public IToken Zero()
        {
            if (!_scanner.MoveNext())
            {
                return new NumberToken(0.0, _errors, _start, _scanner.Position);
            }

            var current = _scanner.Current;

            if (current == CharacterTable.SmallX || current == CharacterTable.BigX)
            {
                return Hex();
            }
            else if (current == CharacterTable.SmallB || current == CharacterTable.BigB)
            {
                return Binary();
            }
            else if (current == CharacterTable.SmallO || current == CharacterTable.BigO)
            {
                return Octal();
            }
            else if (current.IsDigit() || current == CharacterTable.FullStop)
            {
                return Digit();
            }

            return Final();
        }

        public IToken Dot()
        {
            if (_scanner.MoveNext())
            {
                if (_scanner.Current.IsDigit())
                {
                    return Decimal();
                }
                else if (_scanner.Current == CharacterTable.FullStop)
                {
                    return new OperatorToken(TokenType.DotDot, "..", _start);
                }

                _scanner.MoveBack();
            }

            return new OperatorToken(TokenType.Dot, ".", _start);
        }

        public IToken Digit()
        {
            while (_scanner.Current.IsDigit())
            {
                AddValue(10UL, (UInt64)(_scanner.Current - CharacterTable.Zero));

                if (!_scanner.MoveNext())
                {
                    break;
                }
            }

            if (_scanner.Current == CharacterTable.FullStop && _scanner.MoveNext())
            {
                if (_scanner.Current == CharacterTable.FullStop)
                {
                    _scanner.MoveBack();
                    return Final();
                }

                return Decimal();
            }
            else if (_scanner.Current == CharacterTable.SmallE || _scanner.Current == CharacterTable.BigE)
            {
                return Exponent();
            }

            return Final();
        }

        private IToken Binary()
        {
            var numbers = new List<Int32>();
            var weight = 1;

            while (_scanner.MoveNext() && _scanner.Current.IsInRange(CharacterTable.Zero, CharacterTable.One))
            {
                numbers.Add(_scanner.Current - CharacterTable.Zero);
            }

            for (var i = numbers.Count - 1; i >= 0; --i)
            {
                AddValue(1UL, (UInt64)(numbers[i] * weight));
                weight *= 2;
            }

            return FinalForAltInteger();
        }

        private IToken Octal()
        {
            var numbers = new List<Int32>();
            var weight = 1;

            while (_scanner.MoveNext() && _scanner.Current.IsInRange(CharacterTable.Zero, CharacterTable.Seven))
            {
                numbers.Add(_scanner.Current - CharacterTable.Zero);
            }

            for (var i = numbers.Count - 1; i >= 0; --i)
            {
                AddValue(1UL, (UInt64)(numbers[i] * weight));
                weight *= 8;
            }

            return FinalForAltInteger();
        }

        private IToken Hex()
        {
            var numbers = new List<Int32>();
            var weight = 1;

            while (_scanner.MoveNext() && _scanner.Current.IsHex())
            {
                numbers.Add(_scanner.Current.FromHex());
            }

            for (var i = numbers.Count - 1; i >= 0; --i)
            {
                AddValue(1UL, (UInt64)(numbers[i] * weight));
                weight *= 16;
            }

            return FinalForAltInteger();
        }

        private IToken Decimal()
        {
            while (_scanner.Current.IsDigit())
            {
                AddValue(10UL, (UInt64)(_scanner.Current - CharacterTable.Zero));
                _digits++;

                if (!_scanner.MoveNext())
                {
                    break;
                }
            }

            if (_digits == 0)
            {
                if (IsDotOperator(_scanner.Current))
                {
                    //TODO: Right now disabled as we don't consume / handle dot operators in the general tokenizer
                    //_scanner.MoveBack();
                    return Final();
                }

                AddError(ErrorCode.FloatingMismatch, _scanner.Position.From(_start));
            }

            if (_scanner.Current == CharacterTable.SmallE || _scanner.Current == CharacterTable.BigE)
            {
                return Exponent();
            }

            return Final();
        }

        private IToken Exponent()
        {
            var sign = 1;
            var num = 0;

            if (_scanner.MoveNext())
            {
                if (_scanner.Current == CharacterTable.Minus)
                {
                    _scanner.MoveNext();
                    sign = -1;
                }
                else if (_scanner.Current == CharacterTable.Plus)
                {
                    _scanner.MoveNext();
                }

                while (_scanner.Current.IsDigit())
                {
                    num++;
                    _powers = _powers * 10 + _scanner.Current - CharacterTable.Zero;

                    if (!_scanner.MoveNext())
                    {
                        break;
                    }
                }
            }

            if (num == 0)
            {
                AddError(ErrorCode.ScientificMismatch, _scanner.Position.From(_start));
            }

            _powers = sign * _powers;
            return Final();
        }

        private IToken FinalForAltInteger()
        {
            if (_scanner.Current == CharacterTable.FullStop)
            {
                AddError(ErrorCode.DotUnexpected, _scanner.Position.ToRange());
            }

            return Final();
        }

        private IToken Final()
        {
            _scanner.MoveBack();
            return new NumberToken(Number, _errors, _start, _scanner.Position);
        }

        private void AddError(ErrorCode code, ITextRange range)
        {
            _errors ??= [];
            _errors.Add(new ParseError(code, range));
        }

        private void AddValue(UInt64 scale, UInt64 diff)
        {
            if (_shifts > 0)
            {
                _shifts++;
            }
            else if (NumberHelper.TryMultiply(_value, scale, out var product) && NumberHelper.TryAdd(product, diff, out var newValue))
            {
                // assign new value
                _value = newValue;
            }
            else
            {
                // overflow!
                _shifts++;
            }
        }
    }

    #endregion
}
