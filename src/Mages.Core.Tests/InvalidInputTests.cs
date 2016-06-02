namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Ast.Walkers;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class InvalidInputTests
    {
        [Test]
        public void EscapeSequenceClosedDirectlyShouldNotThrowException()
        {
            var expr = "\"\\\"".ToExpression();
            Assert.IsInstanceOf<ConstantExpression>(expr);
            IsInvalid(expr);
        }

        [Test]
        public void EmptyWhileStatementShouldNotThrowException()
        {
            var stmt = "while () {}".ToStatement();
            Assert.IsInstanceOf<WhileStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void NonBodiedWhileStatementShouldNotThrowException()
        {
            var stmt = "while (0)".ToStatement();
            Assert.IsInstanceOf<WhileStatement>(stmt);
            IsInvalid(stmt);
        }

        private static void IsInvalid(IWalkable element)
        {
            var errors = new List<ParseError>();
            var validator = new ValidationTreeWalker(errors);
            element.Accept(validator);
            Assert.IsTrue(errors.Count > 0);
        }
    }
}
