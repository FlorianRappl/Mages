namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Source;
    using Mages.Core.Tokens;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AtomicExpressionTests
    {
        [Test]
        public void UnknownCharacterIsInvalidExpression()
        {
            var source = "$";
            var tokenSource = Generate(source);
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(tokenSource);
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void EmptySourceIsEmptyExpression()
        {
            var source = "";
            var tokenSource = Generate(source);
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(tokenSource);
            Assert.IsInstanceOf<EmptyExpression>(expr);
        }

        [Test]
        public void SpacesSourceIsEmptyExpression()
        {
            var source = "\t \n   ";
            var tokenSource = Generate(source);
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(tokenSource);
            Assert.IsInstanceOf<EmptyExpression>(expr);
        }

        private IEnumerator<IToken> Generate(String source)
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
