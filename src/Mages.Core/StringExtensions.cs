namespace Mages.Core
{
    using Mages.Core.Source;
    using Mages.Core.Tokens;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A number of useful string extensions.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly NumberTokenizer Number = new NumberTokenizer();
        private static readonly StringTokenizer String = new StringTokenizer();
        private static readonly CommentTokenizer Comment = new CommentTokenizer();
        private static readonly GeneralTokenizer Tokenizer = new GeneralTokenizer(Number, String, Comment);

        /// <summary>
        /// Transforms the string to a token iterator.
        /// </summary>
        /// <param name="source">The string.</param>
        /// <returns>The created token iterator.</returns>
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
