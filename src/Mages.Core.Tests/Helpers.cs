namespace Mages.Core.Tests
{
    using Mages.Core.Source;
    using Mages.Core.Tokens;
    using System;
    using System.Collections.Generic;

    static class Helpers
    {
        public static IEnumerator<IToken> ToTokenStream(this String source)
        {
            var scanner = new StringScanner(source);
            var tokenizer = new GeneralTokenizer(new NumberTokenizer(), new StringTokenizer(), new CommentTokenizer());
            var token = default(IToken);

            do
            {
                token = tokenizer.Next(scanner);
                yield return token;
            }
            while (token.Type != TokenType.End);
        }
    }
}
