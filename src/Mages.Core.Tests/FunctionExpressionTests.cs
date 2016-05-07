namespace Mages.Core.Tests
{
    using Mages.Core.Ast.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class FunctionExpressionTests
    {
        [Test]
        public void ParseSimpleFunction()
        {
            var result = "()=>new{}".ToExpression();
            
            Assert.IsInstanceOf<FunctionExpression>(result);

            var fx = (FunctionExpression)result;
            Assert.AreEqual(0, fx.Parameters.Expressions.Length);

            Assert.IsInstanceOf<ObjectExpression>(fx.Body);
        }

        [Test]
        public void ParseSimpleFunctionWithImplicitReturn()
        {
            var result = "()=>2*3".ToExpression();

            Assert.IsInstanceOf<FunctionExpression>(result);

            var fx = (FunctionExpression)result;

            Assert.AreEqual(0, fx.Parameters.Expressions.Length);
            Assert.IsInstanceOf<BinaryExpression.Multiply>(fx.Body);

            var multiply = (BinaryExpression)fx.Body;

            Assert.IsInstanceOf<ConstantExpression>(multiply.LValue);
            Assert.IsInstanceOf<ConstantExpression>(multiply.RValue);
        }

        [Test]
        public void ParseSimpleFunctionWithOneArgument()
        {
            var result = "(x) => new{}".ToExpression();

            Assert.IsInstanceOf<FunctionExpression>(result);

            var fx = (FunctionExpression)result;
            Assert.AreEqual(1, fx.Parameters.Expressions.Length);
            Assert.IsInstanceOf<IdentifierExpression>(fx.Parameters.Expressions[0]);

            var x = (IdentifierExpression)fx.Parameters.Expressions[0];
            Assert.AreEqual("x", x.Name);

            Assert.IsInstanceOf<ObjectExpression>(fx.Body);
        }

        [Test]
        public void ParseSimpleFunctionWithTwoArguments()
        {
            var result = "(x,y)=>new {}".ToExpression();

            Assert.IsInstanceOf<FunctionExpression>(result);

            var fx = (FunctionExpression)result;
            Assert.AreEqual(2, fx.Parameters.Expressions.Length);
            Assert.IsInstanceOf<IdentifierExpression>(fx.Parameters.Expressions[0]);
            Assert.IsInstanceOf<IdentifierExpression>(fx.Parameters.Expressions[1]);

            var x = (IdentifierExpression)fx.Parameters.Expressions[0];
            Assert.AreEqual("x", x.Name);
            var y = (IdentifierExpression)fx.Parameters.Expressions[1];
            Assert.AreEqual("y", y.Name);

            Assert.IsInstanceOf<ObjectExpression>(fx.Body);
        }

        [Test]
        public void ParseSimpleFunctionWithThreeArguments()
        {
            var result = "(x,y, abc)=>new{}".ToExpression();

            Assert.IsInstanceOf<FunctionExpression>(result);

            var fx = (FunctionExpression)result;
            Assert.AreEqual(3, fx.Parameters.Expressions.Length);
            Assert.IsInstanceOf<IdentifierExpression>(fx.Parameters.Expressions[0]);
            Assert.IsInstanceOf<IdentifierExpression>(fx.Parameters.Expressions[1]);
            Assert.IsInstanceOf<IdentifierExpression>(fx.Parameters.Expressions[2]);

            var x = (IdentifierExpression)fx.Parameters.Expressions[0];
            Assert.AreEqual("x", x.Name);
            var y = (IdentifierExpression)fx.Parameters.Expressions[1];
            Assert.AreEqual("y", y.Name);
            var abc = (IdentifierExpression)fx.Parameters.Expressions[2];
            Assert.AreEqual("abc", abc.Name);

            Assert.IsInstanceOf<ObjectExpression>(fx.Body);
        }

        [Test]
        public void ParseSimpleFunctionWithSingleNakedArgument()
        {
            var result = "_=>new{}".ToExpression();

            Assert.IsInstanceOf<FunctionExpression>(result);

            var fx = (FunctionExpression)result;
            Assert.AreEqual(1, fx.Parameters.Expressions.Length);
            Assert.IsInstanceOf<IdentifierExpression>(fx.Parameters.Expressions[0]);

            var underscore = (IdentifierExpression)fx.Parameters.Expressions[0];
            Assert.AreEqual("_", underscore.Name);

            Assert.IsInstanceOf<ObjectExpression>(fx.Body);
        }
    }
}
