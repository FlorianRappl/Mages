namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class OperationTests
    {
        [Test]
        public void BinaryAddWithNumbersYieldsNumber()
        {
            var result = Eval("2 + 3");
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void BinaryPowerWithNumbersYieldsNumber()
        {
            var result = Eval("2^3");
            Assert.AreEqual(8.0, result);
        }

        [Test]
        public void BinarySubtractWithNumbersYieldsNumber()
        {
            var result = Eval("2 - 3.5");
            Assert.AreEqual(-1.5, result);
        }

        [Test]
        public void BinaryMultiplyWithNumbersYieldsNumber()
        {
            var result = Eval("2.5 * 1.5");
            Assert.AreEqual(3.75, result);
        }

        [Test]
        public void BinaryDivideWithNumbersYieldsNumber()
        {
            var result = Eval("4 / 2");
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void BinaryAddWithMatricessYieldsMatrix()
        {
            var result = Eval("[1,2;3,4]+[4,3;2,1]");
            CollectionAssert.AreEqual(new Double[,] { { 5, 5 }, { 5, 5 } }, (Double[,])result);
        }

        [Test]
        public void BinarySubtractWithMatricesYieldsMatrix()
        {
            var result = Eval("[3,2,1]-[1,0,-1]");
            CollectionAssert.AreEqual(new Double[,] { { 2, 2, 2 } }, (Double[,])result);
        }

        [Test]
        public void BinaryMultiplyWithMatricesYieldsMatrix()
        {
            var result = Eval("[1,2;3,4]*[3;5]");
            CollectionAssert.AreEqual(new Double[,] { { 13 }, { 29 } }, (Double[,])result);
        }

        [Test]
        public void BinaryAndWithNumbersYieldsBoolean()
        {
            var result = Eval("2 && 3");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void BinaryAndWithBooleansYieldsBoolean()
        {
            var result = Eval("true && false");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void BinaryOrWithBooleansYieldsBoolean()
        {
            var result = Eval("true || false");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void BinaryAndWithMatricesYieldsMatrix()
        {
            var result = Eval("[1,0;0,0] && [1,1;1,0]");
            CollectionAssert.AreEquivalent(new Double[,] { { 1.0, 0.0 }, { 0.0, 0.0 } }, (Double[,])result);
        }

        [Test]
        public void BinaryOrWithMatricesYieldsMatrix()
        {
            var result = Eval("[1,0;0,0] || [1,1;1,0]");
            CollectionAssert.AreEquivalent(new Double[,] { { 1.0, 1.0 }, { 1.0, 0.0 } }, (Double[,])result);
        }

        [Test]
        public void BinaryEqWithNumbersYieldsBoolean()
        {
            var result = Eval("2 == 3");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void BinaryNeqWithNumbersYieldsBoolean()
        {
            var result = Eval("2 ~= 3");
            Assert.AreEqual(true, result);
        }

        private static Object Eval(String source)
        {
            var engine = new Engine();
            return engine.Interpret(source);
        }
    }
}
