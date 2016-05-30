namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

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

        [Test]
        public void ComparisonFunctionsShouldYieldNumericValue()
        {
            var result = Eval("min(1)");
            Assert.AreEqual(1.0, result);
        }

        [Test]
        public void ComparisonFunctionsShouldReduceRowVectorToNumericValue()
        {
            var result = Eval("max([1,2,30,4,5])");
            Assert.AreEqual(30.0, result);
        }

        [Test]
        public void ComparisonFunctionsShouldReduceColumnVectorToNumericValue()
        {
            var result = Eval("min([1;2;3;-4;5])");
            Assert.AreEqual(-4.0, result);
        }

        [Test]
        public void ComparisonFunctionsShouldReduceMatrixToColumnVector()
        {
            var result = Eval("min([1,2,3;3,4,5])");
            CollectionAssert.AreEquivalent(new Double[,] { { 1.0 }, { 3.0 } }, (Double[,])result);
        }

        [Test]
        public void ComparisonFunctionsOfEmptyMatrixShouldBeAnEmptyMatrix()
        {
            var result = Eval("sort([])");
            CollectionAssert.AreEquivalent(new Double[,] { }, (Double[,])result);
        }

        [Test]
        public void CallAnUnknownFunctionShouldResultInNull()
        {
            var result = Eval("footemp()");
            Assert.IsNull(result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeClassicallyCallableWithRightTypes()
        {
            var foo = Eval("(x, y) => x * y + y") as Function;
            var result = foo.Invoke(new Object[] { 2.0, 3.0 });
            Assert.AreEqual(9.0, result);
        }

        [Test]
        public void CreateMagesFunctionShouldNotBeClassicallyCallableWithoutRightTypes()
        {
            var foo = Eval("(x, y) => x * y + y") as Function;
            var result = foo.Invoke(new Object[] { 2, 3 });
            Assert.IsNull(result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeDirectlyCallableWithRightReturnType()
        {
            var foo = Eval("(x, y) => x * y + y") as Function;
            var result = foo.Call<Double>(2, 3);
            Assert.AreEqual(9.0, result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeDirectlyCallableWithWrongReturnType()
        {
            var foo = Eval("(x, y) => x * y + y") as Function;
            var result = foo.Call<Boolean>(2, 3);
            Assert.AreEqual(default(Boolean), result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeDirectlyCallableWithoutType()
        {
            var foo = Eval("(x, y) => x * y + y") as Function;
            var result = foo.Call(2, 3);
            Assert.AreEqual(9.0, result);
        }

        [Test]
        public void CallStringWithValidIndexYieldsStringWithSingleCharacter()
        {
            var result = Eval("\"test\"(2)");
            Assert.AreEqual("s", result);
        }

        [Test]
        public void CallStringWithIndexOutOfRangeYieldsNothing()
        {
            var result = Eval("\"test\"(4)");
            Assert.IsNull(result);
        }

        [Test]
        public void CallStringWithInvalidIndexYieldsNothing()
        {
            var result = Eval("\"test\"(\"1\")");
            Assert.IsNull(result);
        }

        [Test]
        public void CallStringWithNegativeIndexYieldsNothing()
        {
            var result = Eval("\"test\"(-1)");
            Assert.IsNull(result);
        }

        [Test]
        public void CallObjectWithValidNameYieldsValue()
        {
            var result = Eval("new { a: 29 }(\"a\")");
            Assert.AreEqual(29.0, result);
        }

        [Test]
        public void CallObjectWithUnknownNameYieldsNothing()
        {
            var result = Eval("new { a: 29 }(\"b\")");
            Assert.IsNull(result);
        }

        [Test]
        public void CallObjectWithWithNonStringYieldsValue()
        {
            var result = Eval("new { \"2\": 29 }(2)");
            Assert.AreEqual(29.0, result);
        }

        [Test]
        public void CallEmptyObjectWithUnknownNameYieldsNothing()
        {
            var result = Eval("new { }(\"Test\")");
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithSingleIntegerArgumentYieldsValue()
        {
            var result = Eval("[1,2,3;4,5,6](4)");
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void CallMatrixWithTwoIntegerArgumentsYieldsValue()
        {
            var result = Eval("[1,2,3;4,5,6](1,1)");
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void CallMatrixWithSingleOutOfBoundsArgumentYieldsNothing()
        {
            var result = Eval("[1,2,3;4,5,6](9)");
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithSecondOutOfBoundsArgumentYieldsNothing()
        {
            var result = Eval("[1,2,3;4,5,6](1,3)");
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithFirstOutOfBoundsArgumentYieldsNothing()
        {
            var result = Eval("[1,2,3;4,5,6](3,1)");
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithStringArgumentYieldsNothing()
        {
            var result = Eval("[1,2,3;4,5,6](\"0\")");
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithBooleanArgumentYieldsNothing()
        {
            var result = Eval("[1,2,3;4,5,6](true)");
            Assert.IsNull(result);
        }

        [Test]
        public void CallFunctionWithStatementsReturningObject()
        {
            var result = Eval("((x, y) => { var a = x + y; var b = x - y; new { a: a, b: b}; })(2, 3)");
            var obj = result as IDictionary<String, Object>;
            Assert.IsNotNull(obj);
            Assert.AreEqual(5.0, obj["a"]);
            Assert.AreEqual(-1.0, obj["b"]);
        }

        [Test]
        public void CustomFunctionShouldBeCurried4Times()
        {
            var result = Eval("f = (x,y,z)=>x+y^2+z^3; f()(1)(2)(3)");
            Assert.AreEqual(32.0, result);
        }

        [Test]
        public void CustomFunctionShouldBeCurriedEqualToOriginal()
        {
            var result = Eval("f = (x,y,z)=>x+y^2+z^3; f() == f");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void CustomFunctionShouldBeCurried2Times()
        {
            var result = Eval("f = (x,y,z)=>x+y^2+z^3; f(1)(3,3)");
            Assert.AreEqual(37.0, result);
        }

        [Test]
        public void VariableArgumentsWithImpliedArgsWithoutNaming()
        {
            var result = Eval("f = ()=>length(args); f(1,2,3,\"hi\", true)");
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void VariableArgumentsWithImpliedArgsDespiteNamedArguments()
        {
            var result = Eval("f = (a,b)=>length(args); f(\"hi\", true)");
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void VariableArgumentsAccessWorks()
        {
            var result = Eval("f = ()=>args(2); f(\"hi\", true, 42)");
            Assert.AreEqual(42.0, result);
        }

        [Test]
        public void VariableArgumentsNotExposedIfArgumentsNamedAccordingly()
        {
            var result = Eval("f = (args)=>length(args); f(1, 2, 3, 4)");
            Assert.AreEqual(1.0, result);
        }

        [Test]
        public void VariableArgumentsOverwrittenIfLocalVariableExists()
        {
            var result = Eval("f = ()=>{ var args = 1; length(args); }; f(1, 2, 3, 4)");
            Assert.AreEqual(1.0, result);
        }

        private static Object Eval(String source)
        {
            var engine = new Engine();
            return engine.Interpret(source);
        }
    }
}
