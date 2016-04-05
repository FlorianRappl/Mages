namespace Mages.Core.Tests
{
    using Mages.Core.Source;
    using Mages.Core.Tokens;
    using NUnit.Framework;

    [TestFixture]
    public class NumberTests
    {
        [Test]
        public void NumberScannerZero()
        {
            var source = "0";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(0.0, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerZeroDotZero()
        {
            var source = "0.0";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(0.0, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerInteger()
        {
            var source = "12345678";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(12345678.0, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerExponential()
        {
            var source = "1e2";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(100.0, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerComplex()
        {
            var source = "5i";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            //Assert.AreEqual(5.0, ((ComplexLiteralToken)result).Value);
        }

        [Test]
        public void NumberScannerFloat()
        {
            var source = "1.58";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(1.58, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerZeroFloat()
        {
            var source = "0.012340";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(0.012340, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerHex()
        {
            var source = "0x1a";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(26, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerBinary()
        {
            var source = "0b01100011";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var result = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(result);
            Assert.AreEqual(99, ((NumberToken)result).Value);
        }

        [Test]
        public void NumberScannerDotMultiply()
        {
            var source = "1.*8";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var number = new NumberTokenizer();
            var one = number.Next(scanner);
            var dot = number.Next(scanner);
            var mul = number.Next(scanner);
            var eight = number.Next(scanner);
            Assert.IsInstanceOf<NumberToken>(one);
            Assert.AreEqual(1, ((NumberToken)one).Value);
            Assert.IsInstanceOf<NumberToken>(eight);
            Assert.AreEqual(TokenType.Dot, dot.Type);
            Assert.AreEqual(TokenType.Multiply, mul.Type);
            Assert.AreEqual(8, ((NumberToken)eight).Value);
        }
    }
}
