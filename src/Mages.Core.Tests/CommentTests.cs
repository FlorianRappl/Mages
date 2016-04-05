namespace Mages.Core.Tests
{
    using Mages.Core.Source;
    using Mages.Core.Tokens;
    using NUnit.Framework;

    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void CommentScannerSlash()
        {
            var source = "/";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var comment = new CommentTokenizer();
            var result = comment.Next(scanner);
            Assert.AreEqual(TokenType.RightDivide, result.Type);
        }

        [Test]
        public void CommentScannerLineComment()
        {
            var content = "This is my line";
            var source = "//" + content;
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var comment = new CommentTokenizer();
            var result = comment.Next(scanner);
            Assert.AreEqual(TokenType.LineComment, result.Type);
            Assert.AreEqual(content, result.Payload);
        }

        [Test]
        public void CommentScannerBlockComment()
        {
            var content = "This is my block";
            var source = "/*" + content + "*/";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var comment = new CommentTokenizer();
            var result = comment.Next(scanner);
            Assert.AreEqual(TokenType.BlockComment, result.Type);
            Assert.AreEqual(content, result.Payload);
        }

        [Test]
        public void CommentScannerLineCommentMultiple()
        {
            var content = "This is my line";
            var source = "//" + content + "\n...";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var comment = new CommentTokenizer();
            var result = comment.Next(scanner);
            Assert.AreEqual(TokenType.LineComment, result.Type);
            Assert.AreEqual(content, result.Payload);
        }

        [Test]
        public void CommentScannerBlockCommentMultipleLines()
        {
            var content = "This\nis\nmy\nblock";
            var source = "/*" + content + "*/";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var comment = new CommentTokenizer();
            var result = comment.Next(scanner);
            Assert.AreEqual(TokenType.BlockComment, result.Type);
            Assert.AreEqual(content, result.Payload);
        }

        [Test]
        public void CommentScannerBlockCommentMultiple()
        {
            var content = "This\nis\nmy\nblock";
            var source = "/*" + content + "*/\n...";
            var scanner = new StringScanner(source);
            Assert.IsTrue(scanner.MoveNext());
            var comment = new CommentTokenizer();
            var result = comment.Next(scanner);
            Assert.AreEqual(TokenType.BlockComment, result.Type);
            Assert.AreEqual(content, result.Payload);
        }
    }
}
