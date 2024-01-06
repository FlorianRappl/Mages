namespace Mages.Repl.Tests
{
    using Mages.Core;
    using Mages.Core.Runtime;
    using Mages.Plugins.Modules;
    using Mocks;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    [TestFixture]
    public class FunctionsTests
    {
        [Test]
        public void ProcessObjectIsNotNullAndContainsSomeProperties()
        {
            var ia = new PseudoInteractivity();
            var creator = new MagesCreator(Enumerable.Empty<IModuleFileReader>(), ia);
            var engine = creator.CreateEngine();

            var process = engine.Interpret("process") as IDictionary<String, Object>;
            Assert.IsNotNull(process);
        }

        [Test]
        public void WorksWithBitmapConstructor_Issue118()
        {
            var eng = new Engine();
            eng.SetStatic(typeof(System.Drawing.Bitmap)).WithName("Bitmap");
            var result = eng.Interpret("Bitmap.create(100, 100)");
            Assert.IsNotNull(result);
        }

        [Test]
        public void WorksWithSwitchableScope_Issue119()
        {
            var scope = new SwitchableScope();
            var eng = new Engine(new Configuration
            {
                Scope = scope,
            });
            scope.Current.Add("foo", 4.0);
            scope.AddScope("alt");

            var result1 = eng.Interpret("foo^2");

            scope.ChangeScope("alt");
            scope.Current.Add("foo", 5.0);

            var result2 = eng.Interpret("foo^2");

            scope.ChangeScope("default");
            scope.Current.Add("bar", 1.0);

            var result3 = eng.Interpret("foo+bar");

            Assert.AreEqual(16.0, result1);
            Assert.AreEqual(25.0, result2);
            Assert.AreEqual(5.0, result3);
        }
    }
}
