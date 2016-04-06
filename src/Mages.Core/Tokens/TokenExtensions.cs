namespace Mages.Core.Tokens
{
    using System;
    using System.Collections.Generic;

    static class TokenExtensions
    {
        public static IEnumerator<IToken> NextNonIgnorable(this IEnumerator<IToken> tokens)
        {
            while (tokens.MoveNext() && tokens.Current.Type == TokenType.Space) ;
            return tokens;
        }

        public static Boolean IsEither(this IToken token, TokenType a, TokenType b)
        {
            var type = token.Type;
            return type == a || type == b;
        }

        public static Boolean IsOneOf(this IToken token, TokenType a, TokenType b, TokenType c, TokenType d)
        {
            var type = token.Type;
            return type == a || type == b || type == c || type == d;
        }
    }
}
