namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public void AddingANewMethodInfoCanBeUsed()
        {
            var engine = new Engine();
            var func = new Func<Double, String, Boolean>((n, str) => n == str.Length);
            engine.AddOrReplace("foo", func.Method);

            var result1 = engine.Interpret("foo(2,\"hi\")");
            var result2 = engine.Interpret("foo(2,\"hallo\")");

            Assert.AreEqual(true, result1);
            Assert.AreEqual(false, result2);
        }

        [Test]
        public void AddingANewMethodReturningVoidShouldYieldNull()
        {
            var engine = new Engine();
            var func = new Action<String>(str => Console.WriteLine(str));
            engine.AddOrReplace("hello", func.Method);

            var result = engine.Interpret("hello(\"World\")");

            Assert.AreEqual(null, result);
        }

        [Test]
        public void AddingANewMethodInfoWithArbitraryReturnTypeAndIntParameterCanBeUsed()
        {
            var engine = new Engine();
            var func = new Func<Int32, String, Char>((n, str) => str[n]);
            engine.AddOrReplace("getCharAt", func.Method);

            var result1 = engine.Interpret("getCharAt(1,\"hi\")");
            var result2 = engine.Interpret("getCharAt(2,\"hallo\")");

            Assert.AreEqual("i", result1);
            Assert.AreEqual("l", result2);
        }

        [Test]
        public void AddingANewMethodTakingAVectorAndReturningAList()
        {
            var engine = new Engine();
            var func = new Func<Double[], List<Double>>(vec => vec.Skip(1).Reverse().Take(2).ToList());
            engine.AddOrReplace("bottom", func.Method);

            var result = engine.Interpret("bottom([1,2,3,4,5])");

            CollectionAssert.AreEquivalent(new Double[,]{{ 5, 4 }}, (Double[,])result);
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
