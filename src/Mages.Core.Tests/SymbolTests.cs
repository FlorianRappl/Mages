namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class SymbolTests
    {
        [Test]
        public void ReadUnknownVariable()
        {
            var source = "a";
            Test(source, new[] { "a" });
        }

        [Test]
        public void ReadKnownVariable()
        {
            var source = "a = 1";
            Test(source, new String[] { });
        }

        [Test]
        public void ReadKnownAndUnknownVariablesInGlobalScope()
        {
            var source = "d = 5; a = b + c * d";
            Test(source, new String[] { "b", "c" });
        }

        [Test]
        public void ReadUnknownVariableInLocalScope()
        {
            var source = "f = (x) => x * y; g = f(z)";
            Test(source, new String[] { "y", "z" });
        }

        private static void Test(String source, String[] variables)
        {
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);
            var actual = statements.FindMissingSymbols().Select(m => m.Name).ToArray();

            CollectionAssert.AreEquivalent(variables, actual);
        }
    }
}
