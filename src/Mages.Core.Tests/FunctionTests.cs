namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class FunctionTests
    {
        [Test]
        public void LogicalFunctionsShouldYieldNumericMatrix()
        {
            var result = Eval("isprime([3,4;5,7])");
            CollectionAssert.AreEquivalent(new Double[,] { { 1.0, 0.0 }, { 1.0, 1.0 } }, (Double[,])result);
        }

        [Test]
        public void LogicalFunctionsShouldYieldBooleanValue()
        {
            var result = Eval("isint(9)");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TrigonometricFunctionsShouldYieldNumericVector()
        {
            var result = Eval("sin([0, pi / 4, pi / 2])");
            CollectionAssert.AreEquivalent(new Double[,] { { 0.0, Math.Sin(Math.PI / 4.0), Math.Sin(Math.PI / 2.0) } }, (Double[,])result);
        }

        [Test]
        public void TrigonometricFunctionsShouldYieldNumericValue()
        {
            var result = Eval("cos(1)");
            Assert.AreEqual(Math.Cos(1.0), result);
        }

        private static Object Eval(String source)
        {
            var engine = new Engine();
            return engine.Interpret(source);
        }
    }
}
