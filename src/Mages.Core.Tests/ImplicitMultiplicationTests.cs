namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Types;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ImplicitMultiplicationTests
    {
        [Test]
        public void ExplicitMultiplicationInvolvingNumberAndIdentifier()
        {
            var expr = "2 * x".ToExpression();

            AssertMultiplication(expr, 2.0, "x");
        }

        [Test]
        public void ExplicitMultiplicationInvolvingTwoIdentifiers()
        {
            var expr = "x * y".ToExpression();

            AssertMultiplication(expr, "x", "y");
        }

        [Test]
        public void ImplicitMultiplicationInvolvingNumberAndIdentifierSeparatedBySpace()
        {
            var expr = "2 x".ToExpression();

            AssertMultiplication(expr, 2.0, "x");
        }

        [Test]
        public void ImplicitMultiplicationInvolvingTwoNumbersSeparatedBySpace()
        {
            var expr = "2 3".ToExpression();

            AssertMultiplication(expr, 2.0, 3.0);
        }

        [Test]
        public void ImplicitMultiplicationInvolvingTwoIdentifiersSeparatedBySpace()
        {
            var expr = "x y".ToExpression();

            AssertMultiplication(expr, "x", "y");
        }

        [Test]
        public void ImplicitMultiplicationInvolvingNumberAndIdentifierNotSeparatedBySpace()
        {
            var expr = "2x".ToExpression();

            AssertMultiplication(expr, 2.0, "x");
        }

        [Test]
        public void ImplicitMultiplicationInvolvingNumberAndFunctionCallNotSeparatedBySpace()
        {
            var expr = "6.28sin(x)".ToExpression();

            AssertMultiplication(expr, 6.28, "sin", "x");
        }

        private static void AssertMultiplication(IExpression expr, String leftName, String rightName)
        {
            AssertMultiplication<VariableExpression, VariableExpression>(expr,
                variable => variable.Name == leftName,
                variable => variable.Name == rightName);
        }
        
        private static void AssertMultiplication(IExpression expr, Double value, String name)
        {
            AssertMultiplication<ConstantExpression, VariableExpression>(expr,
                constant => ((Number)constant.Value).Value == value,
                variable => variable.Name == name);
        }

        private static void AssertMultiplication(IExpression expr, Double leftValue, Double rightValue)
        {
            AssertMultiplication<ConstantExpression, ConstantExpression>(expr,
                constant => ((Number)constant.Value).Value == leftValue,
                constant => ((Number)constant.Value).Value == rightValue);
        }

        private static void AssertMultiplication(IExpression expr, Double value, String functionName, String functionArgument)
        {
            AssertMultiplication<ConstantExpression, CallExpression>(expr,
                constant => ((Number)constant.Value).Value == value,
                call => ((VariableExpression)call.Function).Name == functionName && ((VariableExpression)call.Arguments.Arguments[0]).Name == functionArgument);
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
