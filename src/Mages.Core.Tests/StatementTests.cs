namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class StatementTests
    {
        [Test]
        public void ParseTwoAssignmentStatements()
        {
            var source = "d = 5; a = b + c * d";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(2, statements.Count);
            Assert.IsInstanceOf<SimpleStatement>(statements[0]);
            Assert.IsInstanceOf<SimpleStatement>(statements[1]);

            var assignment1 = (statements[0] as SimpleStatement).Expression as AssignmentExpression;
            var assignment2 = (statements[1] as SimpleStatement).Expression as AssignmentExpression;

            Assert.IsNotNull(assignment1);
            Assert.IsNotNull(assignment2);

            Assert.AreEqual("d", assignment1.VariableName);
            Assert.AreEqual("a", assignment2.VariableName);
            Assert.AreEqual(5.0, (Double)((ConstantExpression)assignment1.Value).Value);
            Assert.IsInstanceOf<BinaryExpression.Add>(assignment2.Value);
        }

        [Test]
        public void ParseOneReturnStatementWithEmptyPayload()
        {
            var source = "return";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<ReturnStatement>(statements[0]);

            var return1 = (statements[0] as ReturnStatement).Expression as EmptyExpression;

            Assert.IsNotNull(return1);
        }

        [Test]
        public void ParseOneReturnStatementWithConstantPayload()
        {
            var source = "return 5";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<ReturnStatement>(statements[0]);

            var return1 = (statements[0] as ReturnStatement).Expression as ConstantExpression;

            Assert.IsNotNull(return1);
        }

        [Test]
        public void ParseTwoStatementWhereReturnHasBinaryPayload()
        {
            var source = "var x = 9; return x + pi";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(2, statements.Count);
            Assert.IsInstanceOf<VarStatement>(statements[0]);
            Assert.IsInstanceOf<ReturnStatement>(statements[1]);

            var assignment1 = (statements[0] as VarStatement).Assignment as AssignmentExpression;
            var return1 = (statements[1] as ReturnStatement).Expression as BinaryExpression.Add;

            Assert.IsNotNull(assignment1);
            Assert.IsNotNull(return1);

            Assert.AreEqual("x", assignment1.VariableName);
        }
    }
}
