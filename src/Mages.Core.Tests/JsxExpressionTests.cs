namespace Mages.Core.Tests
{
    using Mages.Core.Ast.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class JsxExpressionTests
    {
        [Test]
        public void JsxSelfClosingTagIsValidExpression()
        {
            var expr = "<foo />".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }

        [Test]
        public void JsxMissingClosingPartIsInvalidExpression()
        {
            var expr = "<foo ".ToExpression();
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void JsxMissingGreaterSymbolIsInvalidExpression()
        {
            var expr = "<foo /".ToExpression();
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void JsxFragmentCannotBeSelfClosed()
        {
            var expr = "</>".ToExpression();
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void JsxClosingIsInvalidExpression()
        {
            var expr = "</".ToExpression();
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void JsxClosingTagNameIsInvalidExpression()
        {
            var expr = "</foo".ToExpression();
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void JsxTagWithoutClosingElementIsInvalidExpression()
        {
            var expr = "<foo>".ToExpression();
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void JsxFragmentIsValidExpression()
        {
            var expr = "<></>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }
    }
}
