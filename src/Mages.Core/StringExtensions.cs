namespace Mages.Core
{
    using Mages.Core.Source;
    using Mages.Core.Tokens;
    using System;
    using System.Collections.Generic;

    public static class StringExtensions
    {
        private static readonly NumberTokenizer Number = new NumberTokenizer();
        private static readonly StringTokenizer String = new StringTokenizer();
        private static readonly CommentTokenizer Comment = new CommentTokenizer();
        private static readonly GeneralTokenizer Tokenizer = new GeneralTokenizer(Number, String, Comment);

        public static IEnumerator<IToken> ToTokenStream(this String source)
        {
            var scanner = new StringScanner(source);
            var token = default(IToken);

            do
            {
                token = Tokenizer.Next(scanner);
                yield return token;
            }
            while (token.Type != TokenType.End);
        }
    }
}
