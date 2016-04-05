namespace Mages.Core.Tests
{
    using Mages.Core.Source;
    using Mages.Core.Tokens;
    using NUnit.Framework;

    [TestFixture]
    public class StringTests
    {
        [Test]
        public void StringScannerEmpty()
        {
            var source = "";
            var scanner = new StringScanner('"' + source + '"');
            Assert.IsTrue(scanner.MoveNext());
            var number = new StringTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<StringToken>(result);
            Assert.AreEqual(source, result.Payload);
        }

        [Test]
        public void StringScannerHallo()
        {
            var source = "Hallo";
            var scanner = new StringScanner('"' + source + '"');
            Assert.IsTrue(scanner.MoveNext());
            var number = new StringTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<StringToken>(result);
            Assert.AreEqual(source, result.Payload);
        }

        [Test]
        public void StringScannerUnicodeCharacterHeart()
        {
            var source = "\\u2764";
            var scanner = new StringScanner('"' + source + '"');
            Assert.IsTrue(scanner.MoveNext());
            var number = new StringTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<StringToken>(result);
            Assert.AreEqual("❤", result.Payload);
        }

        [Test]
        public void StringScannerUnicodeCharacterInfinity()
        {
            var source = "\\u221e";
            var scanner = new StringScanner('"' + source + '"');
            Assert.IsTrue(scanner.MoveNext());
            var number = new StringTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<StringToken>(result);
            Assert.AreEqual("∞", result.Payload);
        }

        [Test]
        public void StringScannerAsciiCharacterTilde()
        {
            var source = "\\x7e";
            var scanner = new StringScanner('"' + source + '"');
            Assert.IsTrue(scanner.MoveNext());
            var number = new StringTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<StringToken>(result);
            Assert.AreEqual("~", result.Payload);
        }
    }
}
