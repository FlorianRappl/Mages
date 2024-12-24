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

        [Test]
        public void JsxWithPropExpressionIsValidExpression()
        {
            var expr = "<foo>bar<bar x={2} /></foo>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }

        [Test]
        public void JsxWithSelfClosingChildIsValidExpression()
        {
            var expr = "<foo>bar<bar /></foo>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }

        [Test]
        public void JsxWithStringPropIsValidExpression()
        {
            var expr = "<foo x=\"2\" />".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }

        [Test]
        public void JsxWithCompleteContentIsValidExpression()
        {
            var expr = "<foo disabled tabIndex={2+3} bla=\"ooo\">\r\n  Hello <strong>World</strong>!\r\n</foo>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }

        [Test]
        public void JsxWithTextAndTagMixtureIsValidExpression()
        {
            var expr = "<foo> Hi!... <strong> dear, friend.. </strong> oh my~ </foo>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }

        [Test]
        public void JsxWithFragmentAndKeywordPropExpression()
        {
            var expr = "<><h1 x-foo-bar={27+19} class=\"yo\">Foo</h1><p>Bar</p></>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
        }

        [Test]
        public void JsxWithInlineFunctionBodyIsValid()
        {
            var expr = "<foo onFoo={() => console.writeln(\"hello\")}>Hello.</foo>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
            var jsx = expr as JsxExpression;
            Assert.AreEqual(1, jsx.Children.Length);
            Assert.AreEqual(1, jsx.Props.Length);
        }

        [Test]
        public void JsxWithGroupedInlineFunctionBodyIsValid()
        {
            var expr = "<foo onFoo={(() => console.writeln(\"hello\"))}>Hello.</foo>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
            var jsx = expr as JsxExpression;
            Assert.AreEqual(1, jsx.Children.Length);
            Assert.AreEqual(1, jsx.Props.Length);
        }

        [Test]
        public void JsxWithInlineFunctionBlockIsValid()
        {
            var expr = "<foo onFoo={() => {console.writeln(\"hello\");}}>Hello.</foo>".ToExpression();
            Assert.IsInstanceOf<JsxExpression>(expr);
            var jsx = expr as JsxExpression;
            Assert.AreEqual(1, jsx.Children.Length);
            Assert.AreEqual(1, jsx.Props.Length);
        }
    }
}
