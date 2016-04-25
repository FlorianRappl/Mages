namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ImplicitMultiplicationTests
    {
        [Test]
        public void ExplicitMultiplicationInvolvingNumberAndIdentifierShouldWork()
        {
            var source = "2 * x";
            var parser = new ExpressionParser();
            var expr = parser.ParseExpression(source);

            AssertMultiplication(expr, 2.0, "x");
        }
        
        private static void AssertMultiplication(IExpression expr, Double value, String name)
        {
            AssertMultiplication<ConstantExpression, VariableExpression>(expr,
                constant => (Double)constant.Value == value,
                variable => variable.Name == name);
        }

        private static void AssertMultiplication<TLeft, TRight>(IExpression expr, Predicate<TLeft> leftChecker, Predicate<TRight> rightChecker)
        {
            Assert.IsInstanceOf<BinaryExpression.Multiply>(expr);

            var binary = (BinaryExpression)expr;

            Assert.IsInstanceOf<TLeft>(binary.LValue);
            Assert.IsInstanceOf<TRight>(binary.RValue);

            Assert.IsTrue(leftChecker.Invoke((TLeft)binary.LValue));
            Assert.IsTrue(rightChecker.Invoke((TRight)binary.RValue));
        }
    }
}
