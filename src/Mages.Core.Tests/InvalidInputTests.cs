namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class InvalidInputTests
    {
        [Test]
        public void EscapeSequenceClosedDirectlyShouldNotThrowException()
        {
            var source = "\"\\\"";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<ConstantExpression>(expr);
        }
    }
}
