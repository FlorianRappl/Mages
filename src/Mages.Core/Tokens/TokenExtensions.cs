namespace Mages.Core.Tokens
{
    using System;
    using System.Collections.Generic;

    static class TokenExtensions
    {
        public static IEnumerator<IToken> NextNonIgnorable(this IEnumerator<IToken> tokens)
        {
            while (tokens.MoveNext() && tokens.Current.IsIgnorable()) ;
            return tokens;
        }

        public static Boolean IsIgnorable(this IToken token)
        {
            var type = token.Type;
            return type == TokenType.Space || type == TokenType.Comment;
        }

        public static Boolean IsNeither(this IToken token, TokenType a, TokenType b)
        {
            var type = token.Type;
            return type != a && type != b;
        }

        public static Boolean IsEither(this IToken token, TokenType a, TokenType b)
        {
            var type = token.Type;
            return type == a || type == b;
        }

        public static Boolean IsOneOf(this IToken token, TokenType a, TokenType b, TokenType c)
        {
            var type = token.Type;
            return type == a || type == b || type == c;
        }

        public static Boolean IsOneOf(this IToken token, TokenType a, TokenType b, TokenType c, TokenType d)
        {
            var type = token.Type;
            return type == a || type == b || type == c || type == d;
        }

        public static Boolean IsOneOf(this IToken token, TokenType a, TokenType b, TokenType c, TokenType d, TokenType e, TokenType f, TokenType g)
        {
            var type = token.Type;
            return type == a || type == b || type == c || type == d || type == e || type == f || type == g;
        }

        public static Boolean Is(this IToken token, String keyword)
        {
            return token.Type == TokenType.Keyword && token.Payload.Equals(keyword, StringComparison.Ordinal);
        }
    }
}
