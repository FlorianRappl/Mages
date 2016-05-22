namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class InterpretationTests
    {
        [Test]
        public void FactorialOfNegativeNumberShouldBeNegative()
        {
            Test("-3!", -6.0);
        }

        [Test]
        public void AddAndMultiplyNumbersShouldYieldRightResult()
        {
            Test("2+3*4", 14.0);
        }

        [Test]
        public void CallPreviouslyCreatedFunction()
        {
            Test("((x, y) => x + y)(2, 3)", 5.0);
        }

        [Test]
        public void CallFunctionCreatedFromFunction()
        {
            Test("(x => y => x + y)(2)(3)", 5.0);
        }

        [Test]
        public void CallFunctionStoredInGlobalVariable()
        {
            var scope = Test("A = (x, y) => x + y; B = A(2, 3); A(4, 3)", 7.0);
            Assert.AreEqual(5.0, scope["B"]);
            Assert.IsInstanceOf<Function>(scope["A"]);
        }

        [Test]
        public void CallFunctionStoredInGlobalFunctionAndCreatedFromFunction()
        {
            var scope = Test("A = x => y => x + y; B = A(2); C = B(3); B(4)", 6.0);
            Assert.AreEqual(5.0, scope["C"]);
            Assert.IsInstanceOf<Function>(scope["A"]);
            Assert.IsInstanceOf<Function>(scope["B"]);
        }

        private IDictionary<String, Object> Test(String sourceCode, Double expected, Double tolerance = 0.0)
        {
            var engine = new Engine();
            var result = engine.Interpret(sourceCode) as Double?;

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value, tolerance);
            return engine.Scope;
        }
    }
}
