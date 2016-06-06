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

        private static Object Eval(String source)
        {
            var engine = new Engine();
            return engine.Interpret(source);
        }
    }
}
