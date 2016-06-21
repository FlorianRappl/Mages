namespace Mages.Core.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class InterpolatedStringTests
    {
        [Test]
        public void UsingInterpolatedStringWithoutPlaceholders()
        {
            var result = "`this is some test\\r\\nfoo`".Eval();
            Assert.AreEqual("this is some test\r\nfoo", result);
        }

        [Test]
        public void UsingInterpolatedStringWithSimplePlaceholder()
        {
            var result = "x = 5; `five is {x}\\r\\nfoo`".Eval();
            Assert.AreEqual("five is 5\r\nfoo", result);
        }

        [Test]
        public void UsingInterpolatedStringWithComputedPlaceholder()
        {
            var result = "`five is {2 + 3}\\r\\nfoo`".Eval();
            Assert.AreEqual("five is 5\r\nfoo", result);
        }

        [Test]
        public void UsingInterpolatedStringWithMemberPlaceholder()
        {
            var result = "`five is {new { a: 5 }.a}\\r\\nfoo`".Eval();
            Assert.AreEqual("five is 5\r\nfoo", result);
        }

        [Test]
        public void UsingInterpolatedStringWithFunctionCallPlaceholder()
        {
            var result = "`five is {add(2, 3)}\\r\\nfoo`".Eval();
            Assert.AreEqual("five is 5\r\nfoo", result);
        }

        [Test]
        public void UsingInterpolatedStringWithCurriedFunctionCallPlaceholder()
        {
            var result = "`five is {multiply(2.5)(2)}\\r\\nfoo`".Eval();
            Assert.AreEqual("five is 5\r\nfoo", result);
        }

        [Test]
        public void UsingInterpolatedStringWithMatrixPlaceholder()
        {
            var result = "`five is {[5]}\\r\\nfoo`".Eval();
            Assert.AreEqual("five is [5]\r\nfoo", result);
        }
    }
}
