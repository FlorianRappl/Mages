namespace Mages.Core.Tokens
{
    using Mages.Core.Source;
    using System;
    using System.Collections.Generic;

    sealed class GeneralTokenizer : ITokenizer
    {
        #region Fields

        private static readonly HashSet<String> Keywords = new HashSet<String>
        {
            "true",
            "false",
            "var",
        };

        private readonly ITokenizer _number;
        private readonly ITokenizer _string;
        private readonly ITokenizer _comment;

        #endregion

        #region ctor

        public GeneralTokenizer(ITokenizer numberTokenizer, ITokenizer stringTokenizer, ITokenizer commentTokenizer)
        {
            _number = numberTokenizer;
            _string = stringTokenizer;
            _comment = commentTokenizer;
        }

        #endregion

        #region Methods

        public IToken Next(IScanner scanner)
        {
            if (scanner.MoveNext())
            {
                var current = scanner.Current;

                if (current.IsWhiteSpaceCharacter())
                {
                    return new CharacterToken(TokenType.Space, current, scanner.Position);
                }
                else if (current.IsNameStart())
                {
                    return ScanName(scanner);
                }
                else if (current.IsDigit())
                {
                    return _number.Next(scanner);
                }
                else
                {
                    return ScanSymbol(scanner);
                }
            }

            return new EndToken(scanner.Position);
        }

        #endregion

        #region States

        private IToken ScanSymbol(IScanner scanner)
        {
            var start = scanner.Position;

            switch (scanner.Current)
            {
                case CharacterTable.FullStop:
                    return _number.Next(scanner);
                case CharacterTable.Comma:
                    return new CharacterToken(TokenType.Comma, CharacterTable.Comma, start);
                case CharacterTable.Colon:
                    return new CharacterToken(TokenType.Colon, CharacterTable.Colon, start);
                case CharacterTable.SemiColon:
                    return new CharacterToken(TokenType.SemiColon, CharacterTable.SemiColon, start);
                case CharacterTable.Plus:
                    if (scanner.MoveNext() && scanner.Current == CharacterTable.Plus)
                    {
                        return new OperatorToken(TokenType.Increment, "++", start, scanner.Position);
                    }
                    
                    return new OperatorToken(TokenType.Add, "+", start);
                case CharacterTable.Minus:
                    if (scanner.MoveNext() && scanner.Current == CharacterTable.Minus)
                    {
                        return new OperatorToken(TokenType.Decrement, "--", start, scanner.Position);
                    }

                    return new OperatorToken(TokenType.Subtract, "-", start);
                case CharacterTable.GreaterThan:
                    if (scanner.MoveNext() && scanner.Current == CharacterTable.Equal)
                    {
                        return new OperatorToken(TokenType.GreaterEqual, ">=", start, scanner.Position);
                    }

                    return new OperatorToken(TokenType.Greater, ">", start);
                case CharacterTable.LessThan:
                    if (scanner.MoveNext() && scanner.Current == CharacterTable.Equal)
                    {
                        return new OperatorToken(TokenType.LessEqual, "<=", start, scanner.Position);
                    }

                    return new OperatorToken(TokenType.Less, "<", start);
                case CharacterTable.CircumflexAccent:
                    return new OperatorToken(TokenType.Power, "^", start);
                case CharacterTable.Tilde:
                    if (scanner.MoveNext() && scanner.Current == CharacterTable.Equal)
                    {
                        return new OperatorToken(TokenType.NotEqual, "~=", start, scanner.Position);
                    }

                    return new OperatorToken(TokenType.Negate, "~", start);
                case CharacterTable.ExclamationMark:
                    return new OperatorToken(TokenType.Factorial, "!", start);
                case CharacterTable.Equal:
                    return ScanEqual(scanner);
                case CharacterTable.Slash:
                    return _comment.Next(scanner);
                case CharacterTable.Backslash:
                    return new OperatorToken(TokenType.LeftDivide, "\\", start);
                case CharacterTable.Asterisk:
                    return new OperatorToken(TokenType.Multiply, "*", start);
                case CharacterTable.Percent:
                    return new OperatorToken(TokenType.Modulo, "%", start);
                case CharacterTable.Pipe:
                    if (scanner.MoveNext() && scanner.Current == CharacterTable.Pipe)
                    {
                        return new OperatorToken(TokenType.Or, "||", start, scanner.Position);
                    }

                    break;
                case CharacterTable.Ampersand:
                    if (scanner.MoveNext() && scanner.Current == CharacterTable.Ampersand)
                    {
                        return new OperatorToken(TokenType.And, "&&", start, scanner.Position);
                    }

                    break;
                case CharacterTable.QuestionMark:
                    return new OperatorToken(TokenType.Condition, "?", start);
                case CharacterTable.SingleQuotationMark:
                    return new OperatorToken(TokenType.Transpose, "'", start);
                case CharacterTable.DoubleQuotationMark:
                    return _string.Next(scanner);
                case CharacterTable.OpenBracket:
                    return new CharacterToken(TokenType.OpenGroup, CharacterTable.OpenBracket, start);
                case CharacterTable.CloseBracket:
                    return new CharacterToken(TokenType.CloseGroup, CharacterTable.CloseBracket, start);
                case CharacterTable.OpenArray:
                    return new CharacterToken(TokenType.OpenList, CharacterTable.OpenArray, start);
                case CharacterTable.CloseArray:
                    return new CharacterToken(TokenType.CloseList, CharacterTable.CloseArray, start);
                case CharacterTable.OpenScope:
                    return new CharacterToken(TokenType.OpenScope, CharacterTable.OpenScope, start);
                case CharacterTable.CloseScope:
                    return new CharacterToken(TokenType.CloseScope, CharacterTable.CloseScope, start);
                case CharacterTable.End:
                    return new EndToken(start);
            }

            return new CharacterToken(TokenType.Unknown, scanner.Current, start);
        }

        private static IToken ScanName(IScanner scanner)
        {
            var position = scanner.Position;
            var sb = StringBuilderPool.Pull();
            var current = scanner.Current;

            do
            {
                sb.Append(Char.ConvertFromUtf32(current));
                
                if (!scanner.MoveNext())
                {
                    break;
                }

                current = scanner.Current;
            }
            while (current.IsName());

            var name = sb.Stringify();
            var isKeyword = Keywords.Contains(name);
            var type = isKeyword ? TokenType.Keyword : TokenType.Identifier;
            return new IdentToken(type, name, position, scanner.Position);
        }

        private static IToken ScanEqual(IScanner scanner)
        {
            var position = scanner.Position;

            if (scanner.MoveNext())
            {
                var current = scanner.Current;

                if (current == CharacterTable.Equal)
                {
                    return new OperatorToken(TokenType.Equal, "==", position, scanner.Position);
                }
                else if (current == CharacterTable.GreaterThan)
                {
                    return new OperatorToken(TokenType.Lambda, "=>", position, scanner.Position);
                }
            }

            return new OperatorToken(TokenType.Assignment, "=", position);
        }

        #endregion
    }
}