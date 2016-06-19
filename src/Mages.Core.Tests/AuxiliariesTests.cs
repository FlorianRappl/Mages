namespace Mages.Core.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class AuxiliariesTests
    {
        [Test]
        public void ReduceWithNumberShouldYieldSingleCall()
        {
            var result = "reduce(add, 2, 3)".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void ReduceWithBooleanShouldYieldSingleCall()
        {
            var result = "reduce(and, true, false)".Eval();
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ReduceWithMatrixShouldYieldOneValue()
        {
            var result = "reduce(add, 0, [1, 2, 3, 4, 5, 6])".Eval();
            Assert.AreEqual(21.0, result);
        }

        [Test]
        public void ReduceWitObjectShouldYieldOneValue()
        {
            var result = "reduce(multiply, 1, new { a: 5, b: 4, c: 3 })".Eval();
            Assert.AreEqual(60.0, result);
        }
    }
}
