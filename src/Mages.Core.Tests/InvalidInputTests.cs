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
        public void BareWhileStatementShouldNotThrowException()
        {
            var stmt = "while true {}".ToStatement();
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

        [Test]
        public void ScopeInConditionOfWhileStatementShouldNotThrowException()
        {
            var stmt = "while (0 {}) {}".ToStatement();
            Assert.IsInstanceOf<WhileStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithMissingOperandsShouldFail()
        {
            var stmt = "for() { }".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithIncompleteBlockShouldFail()
        {
            var stmt = "for() { ".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithMissingRestShouldFail()
        {
            var stmt = "for(k=0 { }".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithCommasInHeadShouldFail()
        {
            var stmt = "for(k=0, k ~= 2, k++) { }".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithIncompleteInitializationShouldFail()
        {
            var stmt = "for(k=; k ~= 2 ; k++) { }".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithStatementInInitializationShouldFail()
        {
            var stmt = "for(break; k ~= 2 ; k++) { }".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithStatementInAfterThoughtShouldFail()
        {
            var stmt = "for(; ; var k = 0) { }".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void ForStatementWithoutBodyShouldFail()
        {
            var stmt = "for(; ; )".ToStatement();
            Assert.IsInstanceOf<ForStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void DeleteWithoutPayloadIsInvalid()
        {
            var stmt = "delete".ToStatement();
            Assert.IsInstanceOf<DeleteStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void DeleteWithNumberPayloadIsInvalid()
        {
            var stmt = "delete 2".ToStatement();
            Assert.IsInstanceOf<DeleteStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void DeleteWithCallExpressionPayloadIsInvalid()
        {
            var stmt = "delete foo()".ToStatement();
            Assert.IsInstanceOf<DeleteStatement>(stmt);
            IsInvalid(stmt);
        }

        [Test]
        public void DeleteWithBinaryExpressionPayloadIsInvalid()
        {
            var stmt = "delete foo+bar".ToStatement();
            Assert.IsInstanceOf<DeleteStatement>(stmt);
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
