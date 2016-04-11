namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class MatrixExpressionTests
    {
        [Test]
        public void EmptyMatrix()
        {
            var source = @"[]";
            var tokens = source.ToTokenStream();
            var parser = new ExpressionParser();
            var result = parser.ParseExpression(tokens);

            Assert.IsInstanceOf<MatrixExpression>(result);

            var matrix = (MatrixExpression)result;

            Assert.AreEqual(0, matrix.Values.Length);
        }

        [Test]
        public void SingleElementMatrix()
        {
            var source = @"[2]";
            var tokens = source.ToTokenStream();
            var parser = new ExpressionParser();
            var result = parser.ParseExpression(tokens);

            Assert.IsInstanceOf<MatrixExpression>(result);

            var matrix = (MatrixExpression)result;

            Assert.AreEqual(1, matrix.Values.Length);
            Assert.AreEqual(1, matrix.Values[0].Length);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][0]);
        }

        [Test]
        public void SingleVectorMatrix()
        {
            var source = @"[1,2,3]";
            var tokens = source.ToTokenStream();
            var parser = new ExpressionParser();
            var result = parser.ParseExpression(tokens);

            Assert.IsInstanceOf<MatrixExpression>(result);

            var matrix = (MatrixExpression)result;

            Assert.AreEqual(1, matrix.Values.Length);
            Assert.AreEqual(3, matrix.Values[0].Length);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][0]);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][1]);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][2]);
        }

        [Test]
        public void VectorWithSpacesMatrix()
        {
            var source = @"[1,2  ,   3,4];";
            var tokens = source.ToTokenStream();
            var parser = new ExpressionParser();
            var result = parser.ParseExpression(tokens);

            Assert.IsInstanceOf<MatrixExpression>(result);

            var matrix = (MatrixExpression)result;

            Assert.AreEqual(1, matrix.Values.Length);
            Assert.AreEqual(4, matrix.Values[0].Length);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][0]);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][1]);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][2]);
            Assert.IsInstanceOf<ConstantExpression>(matrix.Values[0][3]);
        }

        [Test]
        public void DifferentExpressionsInVectorMatrix()
        {
            var source = @"[1+3,x,f(3),7*3];";
            var tokens = source.ToTokenStream();
            var parser = new ExpressionParser();
            var result = parser.ParseExpression(tokens);

            Assert.IsInstanceOf<MatrixExpression>(result);

            var matrix = (MatrixExpression)result;

            Assert.AreEqual(1, matrix.Values.Length);
            Assert.AreEqual(4, matrix.Values[0].Length);
            Assert.IsInstanceOf<BinaryExpression.Add>(matrix.Values[0][0]);
            Assert.IsInstanceOf<VariableExpression>(matrix.Values[0][1]);
            Assert.IsInstanceOf<CallExpression>(matrix.Values[0][2]);
            Assert.IsInstanceOf<BinaryExpression.Multiply>(matrix.Values[0][3]);
        }

        [Test]
        public void FunctionVectorAndArithmeticInVectorMatrix()
        {
            var source = @"[()=>3,[1,2,3,4],2+3,(1-2)*3];";
            var tokens = source.ToTokenStream();
            var parser = new ExpressionParser();
            var result = parser.ParseExpression(tokens);

            Assert.IsInstanceOf<MatrixExpression>(result);

            var matrix = (MatrixExpression)result;

            Assert.AreEqual(1, matrix.Values.Length);
            Assert.AreEqual(4, matrix.Values[0].Length);
            Assert.IsInstanceOf<FunctionExpression>(matrix.Values[0][0]);
            Assert.IsInstanceOf<MatrixExpression>(matrix.Values[0][1]);
            Assert.IsInstanceOf<BinaryExpression.Add>(matrix.Values[0][2]);
            Assert.IsInstanceOf<BinaryExpression.Multiply>(matrix.Values[0][3]);
        }
    }
}
