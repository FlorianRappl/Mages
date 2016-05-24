namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;

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

        private static Object Eval(String source)
        {
            var engine = new Engine();
            return engine.Interpret(source);
        }
    }
}
