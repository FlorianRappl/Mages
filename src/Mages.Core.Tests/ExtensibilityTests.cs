namespace Mages.Core.Tests
{
    using Mages.Core.Runtime;
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
            engine.SetFunction("foo", function);

            var result = engine.Interpret("foo(1,2,3)");
            Assert.AreEqual(3.0, result);
        }

        [Test]
        public void AddingANewDelegateCanBeUsed()
        {
            var engine = new Engine();
            var func = new Func<Double, String, Boolean>((n, str) => n == str.Length);
            engine.SetFunction("foo", func);

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
            engine.SetFunction("foo", func.Method, func.Target);

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
            engine.SetFunction("hello", func.Method, func.Target);

            var result = engine.Interpret("hello(\"World\")");

            Assert.AreEqual(null, result);
        }

        [Test]
        public void AddingANewMethodInfoWithArbitraryReturnTypeAndIntParameterCanBeUsed()
        {
            var engine = new Engine();
            var func = new Func<Int32, String, Char>((n, str) => str[n]);
            engine.SetFunction("getCharAt", func.Method, func.Target);

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
            engine.SetFunction("bottom", func.Method, func.Target);

            var result = engine.Interpret("bottom([1,2,3,4,5])");

            CollectionAssert.AreEquivalent(new Double[,] { { 5, 4 } }, (Double[,])result);
        }

        [Test]
        public void ReplacingAnExistingFunctionOverwrites()
        {
            var engine = new Engine();
            var function = new Function(args => (Double)args.Length);
            engine.SetFunction("sin", function);

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

        [Test]
        public void AddingABooleanConstantShouldNotBeConverted()
        {
            var engine = new Engine();
            engine.SetConstant("foo", true);

            var result = engine.Interpret("foo");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void AddingANumberConstantShouldNotBeConverted()
        {
            var engine = new Engine();
            engine.SetConstant("foo", 2.3);

            var result = engine.Interpret("foo");
            Assert.AreEqual(2.3, result);
        }

        [Test]
        public void AddingAnIntegerConstantShouldBeConvertedToANumber()
        {
            var engine = new Engine();
            engine.SetConstant("foo", 2);

            var result = engine.Interpret("foo");
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void AddingASingleConstantShouldBeConvertedToANumber()
        {
            var engine = new Engine();
            engine.SetConstant("foo", 2f);

            var result = engine.Interpret("foo");
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void AddingAStringConstantShouldNotBeConverted()
        {
            var engine = new Engine();
            var str = "hallo";
            engine.SetConstant("foo", str);

            var result = engine.Interpret("foo");
            Assert.AreEqual(str, result);
        }

        [Test]
        public void AddingACharacterConstantShouldBeConvertedToAString()
        {
            var engine = new Engine();
            engine.SetConstant("foo", 'c');

            var result = engine.Interpret("foo");
            Assert.AreEqual("c", result);
        }

        [Test]
        public void AddingAMatrixConstantShouldNotBeConverted()
        {
            var engine = new Engine();
            var matrix = new Double[,] { { 1, 2 }, { 3, 4 } };
            engine.SetConstant("foo", matrix);

            var result = engine.Interpret("foo");
            Assert.AreEqual(matrix, result);
        }

        [Test]
        public void AddingADictionaryConstantShouldNotBeConverted()
        {
            var engine = new Engine();
            var obj = new Dictionary<String, Object>
            {
                { "a", true },
                { "b", "hallo" }
            };
            engine.SetConstant("foo", obj);

            var result = engine.Interpret("foo");
            Assert.AreEqual(obj, result);
        }

        [Test]
        public void AddingAPointConstantShouldBeConvertedToAWrapper()
        {
            var engine = new Engine();
            var pt = new Point();
            engine.SetConstant("foo", pt);

            var result = engine.Interpret("foo");

            Assert.IsInstanceOf<WrapperObject>(result);

            var wrapper = (WrapperObject)result;
            Assert.AreEqual(pt, wrapper.Content);
        }

        sealed class Point
        {
            public Double x;
            public Double y;
        }
    }
}
