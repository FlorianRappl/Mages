namespace Mages.Core.Tests
{
    using Mages.Core.Ast.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class InvalidInputTests
    {
        [Test]
        public void EscapeSequenceClosedDirectlyShouldNotThrowException()
        {
            var expr = "\"\\\"".ToExpression();
            Assert.IsInstanceOf<ConstantExpression>(expr);
        }
    }
}
