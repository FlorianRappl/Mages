namespace Mages.Repl.Tests
{
    using Mages.Core;
    using Mages.Plugins.Modules;
    using Mocks;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
