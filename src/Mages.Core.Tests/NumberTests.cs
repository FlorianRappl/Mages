using Mages.Core.Source;
using Mages.Core.Tokens;
using NUnit.Framework;
using System.Linq;

namespace Mages.Core.Tests;

[TestFixture]
public class NumberTests
{
    [Test]
    public void NumberScannerZero()
    {
        var source = "0";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(0.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerZeroDotZero()
    {
        var source = "0.0";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(0.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerTrailingDot()
    {
        var source = "3.";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(3.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerScientificMinus()
    {
        var source = "1e-1";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(0.1, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerLargeValue()
    {
        var source = "9223372036854775807";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(9223372036854776000.0, ((NumberToken)result).Value);
    }

    [TestCase("0.8409014139716477191")]
    [TestCase("0.84090141397164771912")]
    public void NumberScannerLargePrecisionValue(string source)
    {
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        // double has a precision of ~15-17 digits
        Assert.AreEqual(0.840901413971647, ((NumberToken)result).Value, 1e-15);
    }

    [Test]
    public void NumberScannerScientificPlus()
    {
        var source = "1e+1";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(10.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerScientificDotShouldStop()
    {
        var source = "1e1.2";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(10.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerInteger()
    {
        var source = "12345678";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(12345678.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerExponential()
    {
        var source = "1e2";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(100.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerComplex()
    {
        var source = "5i";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(5.0, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerFloat()
    {
        var source = "1.58";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(1.58, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerZeroFloat()
    {
        var source = "0.012340";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(0.012340, ((NumberToken)result).Value);
    }

    [Test]
    public void NumberScannerHex()
    {
        var source = "0x1a";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(26, ((NumberToken)result).Value);
        Assert.IsFalse(((NumberToken)result).Errors?.Any() ?? false);
    }

    [Test]
    public void NumberScannerHexWithFractionInvalid_Issue128()
    {
        var source = "0x1a.1";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(26, ((NumberToken)result).Value);
        Assert.IsTrue(((NumberToken)result).Errors?.Any() ?? false);
    }

    [Test]
    public void NumberScannerBinary()
    {
        var source = "0b01100011";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(99, ((NumberToken)result).Value);
        Assert.IsFalse(((NumberToken)result).Errors?.Any() ?? false);
    }

    [Test]
    public void NumberScannerBinaryWithFractionInvalid_Issue128()
    {
        var source = "0b01100011.1";
        var scanner = new StringScanner(source);
        Assert.IsTrue(scanner.MoveNext());
        var tokenizer = new NumberTokenizer();
        var result = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(result);
        Assert.AreEqual(99, ((NumberToken)result).Value);
        Assert.IsTrue(((NumberToken)result).Errors?.Any() ?? false);
    }

    [Test]
    public void NumberScannerDotMultiply()
    {
        var source = "1.*8";
        var scanner = new StringScanner(source);
        var tokenizer = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var one = tokenizer.Next(scanner);
        var mul = tokenizer.Next(scanner);
        var eight = tokenizer.Next(scanner);
        Assert.IsInstanceOf<NumberToken>(one);
        Assert.AreEqual(1, ((NumberToken)one).Value);
        Assert.AreEqual(TokenType.Multiply, mul.Type);
        Assert.IsInstanceOf<NumberToken>(eight);
        Assert.AreEqual(8, ((NumberToken)eight).Value);
    }

    [Test]
    public void NumberAfterAndBeforeSpace()
    {
        var source = " 1.2e5 ";
        var scanner = new StringScanner(source);
        var comment = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var space1 = comment.Next(scanner);
        var result = comment.Next(scanner);
        var space2 = comment.Next(scanner);
        var end = comment.Next(scanner);
        Assert.AreEqual(TokenType.Space, space1.Type);
        Assert.AreEqual(TokenType.Number, result.Type);
        Assert.AreEqual(TokenType.Space, space2.Type);
        Assert.AreEqual(TokenType.End, end.Type);
        Assert.AreEqual("120000", result.Payload);
    }

    [Test]
    public void DigitsOnlyNumberWithOverflowForInt32()
    {
        var source = "0.212410080106903";
        var scanner = new StringScanner(source);
        var comment = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var result = comment.Next(scanner);
        var end = comment.Next(scanner);
        Assert.AreEqual(TokenType.Number, result.Type);
        Assert.AreEqual(TokenType.End, end.Type);
        Assert.AreEqual("0.21241008010690302", result.Payload);
    }

    [Test]
    public void IntegerDigitsNumberWithOverflowForInt32()
    {
        var source = "131.208072980527";
        var scanner = new StringScanner(source);
        var comment = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var result = comment.Next(scanner);
        var end = comment.Next(scanner);
        Assert.AreEqual(TokenType.Number, result.Type);
        Assert.AreEqual(TokenType.End, end.Type);
        Assert.AreEqual("131.208072980527", result.Payload);
    }

    [Test]
    public void InputNumberTooLarge_Issue110()
    {
        var source = "55555555555555555555";
        var scanner = new StringScanner(source);
        var comment = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var result = comment.Next(scanner);
        var end = comment.Next(scanner);
        Assert.AreEqual(TokenType.Number, result.Type);
        Assert.AreEqual(TokenType.End, end.Type);
        Assert.AreEqual(55555555555555555555.0, ((NumberToken)result).Value);
    }

    [Test]
    public void InputNumberTooLargeMore_Issue110()
    {
        var source = "555555555555555555555";
        var scanner = new StringScanner(source);
        var comment = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var result = comment.Next(scanner);
        var end = comment.Next(scanner);
        Assert.AreEqual(TokenType.Number, result.Type);
        Assert.AreEqual(TokenType.End, end.Type);
        Assert.AreEqual(555555555555555555555.0, ((NumberToken)result).Value);
    }

    [Test]
    public void InputNumberTooLargeLess_Issue110()
    {
        var source = "0.555555555555555555555";
        var scanner = new StringScanner(source);
        var comment = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var result = comment.Next(scanner);
        var end = comment.Next(scanner);
        Assert.AreEqual(TokenType.Number, result.Type);
        Assert.AreEqual(TokenType.End, end.Type);
        Assert.AreEqual(0.55555555555555547, ((NumberToken)result).Value);
    }

    [Test]
    public void InputNumberTooLargeLesser_Issue110()
    {
        var source = "0.55555555555555555555555";
        var scanner = new StringScanner(source);
        var comment = new GeneralTokenizer(new NumberTokenizer(), null, null);
        var result = comment.Next(scanner);
        var end = comment.Next(scanner);
        Assert.AreEqual(TokenType.Number, result.Type);
        Assert.AreEqual(TokenType.End, end.Type);
        Assert.AreEqual(0.55555555555555547, ((NumberToken)result).Value);
    }
}
