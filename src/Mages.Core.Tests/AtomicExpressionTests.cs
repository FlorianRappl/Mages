namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class AtomicExpressionTests
    {
        [Test]
        public void UnknownCharacterIsInvalidExpression()
        {
            var source = "$";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<InvalidExpression>(expr);
        }

        [Test]
        public void EmptySourceIsEmptyExpression()
        {
            var source = "";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<EmptyExpression>(expr);
        }

        [Test]
        public void SemicolonIsEmptyExpression()
        {
            var source = ";";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<EmptyExpression>(expr);
        }

        [Test]
        public void SpacesSourceIsEmptyExpression()
        {
            var source = "\t \n   ";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<EmptyExpression>(expr);
        }

        [Test]
        public void UnknownCharacterIsInvalidExpressionContainedInBinaryExpression()
        {
            var source = "$+";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<BinaryExpression>(expr);
            var left = ((BinaryExpression)expr).LValue;
            var right = ((BinaryExpression)expr).RValue;
            Assert.IsInstanceOf<InvalidExpression>(left);
            Assert.IsInstanceOf<EmptyExpression>(right);
        }

        [Test]
        public void TrueIsConstantExpression()
        {
            var source = "true";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<ConstantExpression>(expr);
        }

        [Test]
        public void FalseIsConstantExpression()
        {
            var source = "false";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<ConstantExpression>(expr);
        }

        [Test]
        public void ArbitraryIdentifierIsVariableExpression()
        {
            var source = "a";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<VariableExpression>(expr);
        }

        [Test]
        public void NumberIsConstantExpression()
        {
            var source = "2.3";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<ConstantExpression>(expr);
        }

        [Test]
        public void StringIsConstantExpression()
        {
            var source = "\"hi there\"";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);
            Assert.IsInstanceOf<ConstantExpression>(expr);
        }
    }
}
