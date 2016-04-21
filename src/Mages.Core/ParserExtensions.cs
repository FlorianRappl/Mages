namespace Mages.Core
{
    using Mages.Core.Ast;
    using System;

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
    }
}
