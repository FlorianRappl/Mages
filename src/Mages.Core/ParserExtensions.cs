namespace Mages.Core
{
    using Mages.Core.Ast;
    using Mages.Core.Tokens;
    using System;
    using System.Collections.Generic;

    public static class ParserExtensions
    {
        public static IExpression ParseExpression(this IParser parser, String code)
        {
            var stream = code.ToTokenStream();
            return parser.ParseExpression(stream);
        }

        public static IStatement ParseStatement(this IParser parser, String code)
        {
            var stream = code.ToTokenStream();
            return parser.ParseStatement(stream);
        }

        public static List<IStatement> ParseStatements(this IParser parser, String code)
        {
            var stream = code.ToTokenStream();
            return parser.ParseStatements(stream);
        }

        public static List<IStatement> ParseStatements(this IParser parser, IEnumerator<IToken> tokens)
        {
            var statements = new List<IStatement>();

            do
            {
                var statement = parser.ParseStatement(tokens);
                statements.Add(statement);
            }
            while (tokens.Current.Type == TokenType.SemiColon);

            return statements;
        }
    }
}
