namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ExtensibilityTests
    {
        [Test]
        public void AddingANewFunctionCanBeUsed()
        {
            var engine = new Engine();
            var function = new Function(args => (Double)args.Length);
            engine.AddOrReplace("foo", function);

            var result = engine.Interpret("foo(1,2,3)");
            Assert.AreEqual(3.0, result);
        }

        [Test]
        public void AddingANewDelegateCanBeUsed()
        {
            var engine = new Engine();
            var func = new Func<Double, String, Boolean>((n, str) => n == str.Length);
            engine.AddOrReplace("foo", func);

            var result1 = engine.Interpret("foo(2,\"hi\")");
            var result2 = engine.Interpret("foo(2,\"hallo\")");

            Assert.AreEqual(true, result1);
            Assert.AreEqual(false, result2);
        }

        [Test]
        public void ReplacingAnExistingFunctionOverwrites()
        {
            var engine = new Engine();
            var function = new Function(args => (Double)args.Length);
            engine.AddOrReplace("sin", function);

            var result = engine.Interpret("sin(1,2)");
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void AddingAGlobalConstant()
        {
            var engine = new Engine();
            engine.Globals["Answer"] = 42.0;

            var result = engine.Interpret("Answer / 2");
            Assert.AreEqual(21.0, result);
        }
    }
}
