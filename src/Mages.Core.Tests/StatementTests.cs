namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using NUnit.Framework;

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
            Assert.AreEqual(5.0, ((ConstantExpression)assignment1.Value).Value);
            Assert.IsInstanceOf<BinaryExpression.Add>(assignment2.Value);
        }
    }
}
