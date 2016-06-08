namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class PluginTests
    {
        [Test]
        public void AddingNewPluginShouldNotOverrideExistingObjects()
        {
            var metaData = new Dictionary<String, String>();
            var content = new Dictionary<String, Object>();
            content["sin"] = 3.0;
            var plugin = new Plugin(metaData, content);
            var engine = new Engine();
            engine.AddPlugin(plugin);

            var sin = engine.Interpret("sin");

            Assert.AreNotEqual(content["sin"], sin);
        }

        [Test]
        public void AddingNewPluginShouldAddNewObjects()
        {
            var metaData = new Dictionary<String, String>();
            var content = new Dictionary<String, Object>();
            content["foo"] = 3.0;
            var plugin = new Plugin(metaData, content);
            var engine = new Engine();
            engine.AddPlugin(plugin);

            var foo = engine.Interpret("foo");

            Assert.AreEqual(content["foo"], foo);
        }

        [Test]
        public void AddingNewPluginShouldAddAllNew()
        {
            var metaData = new Dictionary<String, String>();
            var content = new Dictionary<String, Object>();
            content["a"] = new Function(args => (Double)args.Length);
            content["b"] = 2.0;
            var plugin = new Plugin(metaData, content);
            var engine = new Engine();
            engine.AddPlugin(plugin);

            var five = engine.Interpret("a(1, 2, 3) + b");

            Assert.AreEqual(5.0, five);
        }

        [Test]
        public void RemovingInexistingPluginHasNoEffect()
        {
            var metaData = new Dictionary<String, String>();
            var content = new Dictionary<String, Object>();
            content["a"] = new Function(args => (Double)args.Length);
            var plugin = new Plugin(metaData, content);
            var engine = new Engine();
            engine.Globals["a"] = content["a"];
            engine.RemovePlugin(plugin);

            var three = engine.Interpret("a(1, 2, 3)");

            Assert.AreEqual(3.0, three);
        }

        [Test]
        public void RemovingExistingPluginRemovesNewFunction()
        {
            var metaData = new Dictionary<String, String>();
            var content = new Dictionary<String, Object>();
            content["a"] = new Function(args => (Double)args.Length);
            var plugin = new Plugin(metaData, content);
            var engine = new Engine();
            engine.AddPlugin(plugin);
            engine.RemovePlugin(plugin);

            var undefined = engine.Interpret("a(1, 2, 3)");

            Assert.AreEqual(null, undefined);
        }
    }
}
