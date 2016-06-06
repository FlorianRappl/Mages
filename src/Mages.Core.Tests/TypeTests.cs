namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TypeTests
    {
        [Test]
        public void IsFunctionCanBeCurriedStringIsString()
        {
            var result = Eval("is_string = is(\"String\"); is_string()()(\"Hi\")");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsFunctionCanBeCurriedNumberAintString()
        {
            var result = Eval("is_number = is(\"Number\"); is_number()()(\"Hi\")");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AsFunctionCanBeCurriedNumberAsString()
        {
            var result = Eval("caster = as(\"String\"); caster()()(23.6)");
            Assert.AreEqual("23.6", result);
        }

        [Test]
        public void AsFunctionCanBeCurriedBooleanAsNumber()
        {
            var result = Eval("cast = as(\"Number\"); cast()(true)");
            Assert.AreEqual(1.0, result);
        }

        [Test]
        public void IsFunctionStringIsString()
        {
            var result = Eval("is(\"String\", \"Test string\") // true");
            Assert.AreEqual(true, result);
        }
                
        [Test]
        public void IsFunctionBooleanAintNumber()
        {
            var result = Eval("is(\"Number\", true) // false");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsFunctionBooleanExpressionIsCurriedBoolean()
        {
            var result = Eval("is_bool = is(\"Boolean\"); is_bool(14 ~= 7) // true");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void AsFunctionConvertsNumberToString()
        {
            var result = Eval("as(\"String\", 3.5) // \"3.5\"");
            Assert.AreEqual("3.5", result);
        }

        [Test]
        public void AsFunctionConvertsBooleanToMatrix()
        {
            var result = Eval("as(\"Matrix\", true) // [1]");
            CollectionAssert.AreEquivalent(new Double[,] { { 1.0 } }, (Double[,])result);
        }

        [Test]
        public void AsFunctionConvertsNullToString()
        {
            var result = Eval("as(\"String\", null)");
            Assert.AreEqual(String.Empty, result);
        }

        [Test]
        public void AsFunctionConvertsStringToNull()
        {
            var result = Eval("as(\"Undefined\", \"Hi\")");
            Assert.AreEqual(null, result);
        }

        private static Object Eval(String source)
        {
            var engine = new Engine();
            return engine.Interpret(source);
        }
    }
}
