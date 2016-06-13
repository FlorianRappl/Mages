namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class IntellisenseTests
    {
        [Test]
        public void EmptyScopeAndSourceYieldsKeywords()
        {
            var source = "";
            var engine = new Engine();
            engine.Globals.Clear();
            var autocomplete = engine.GetCompletionAt(source, 0).ToArray();
            var available = Keywords.GlobalStatementKeywords.Concat(Keywords.ExpressionKeywords);

            CollectionAssert.AreEquivalent(available, autocomplete);
        }

        [Test]
        public void GlobalScopeAndEmptySourceYieldsKeywordsAndVariables()
        {
            var source = "";
            var engine = new Engine();
            var autocomplete = engine.GetCompletionAt(source, 0).ToArray();
            var available = Keywords.GlobalStatementKeywords.Concat(Keywords.ExpressionKeywords).Concat(engine.Globals.Keys);

            CollectionAssert.AreEquivalent(available, autocomplete);
        }

        [Test]
        public void LocalScopeYieldsKeywordsAndLocalVariables()
        {
            var source = "(() => { var x = 5; })";
            var engine = new Engine();
            engine.Globals.Clear();
            var autocomplete = engine.GetCompletionAt(source, source.Length - 3).ToArray();
            var available = Keywords.GlobalStatementKeywords.Concat(Keywords.ExpressionKeywords).Concat(new[] { "x" });

            CollectionAssert.AreEquivalent(available, autocomplete);
        }

        [Test]
        public void OutsideLocalScopeYieldsKeywordsAndVariables()
        {
            var source = "(() => { var x = 5; })";
            var engine = new Engine();
            engine.Globals.Clear();
            var autocomplete = engine.GetCompletionAt(source, source.Length).ToArray();
            var available = Keywords.GlobalStatementKeywords.Concat(Keywords.ExpressionKeywords);

            CollectionAssert.AreEquivalent(available, autocomplete);
        }

        [Test]
        public void InGlobalScopeAfterAssignmentRightHandSideYieldsVariables()
        {
            var source = "x = 5; var y = 9; 7 +";
            var engine = new Engine();
            engine.Globals.Clear();
            var autocomplete = engine.GetCompletionAt(source, source.Length).ToArray();
            var available = Keywords.ExpressionKeywords.Concat(new []{ "x", "y" });

            CollectionAssert.AreEquivalent(available, autocomplete);
        }

        [Test]
        public void ParameterOfFunctionShouldYieldNoCompletion()
        {
            var source = "((a, b, c) => { var x = 5; })";
            var engine = new Engine();
            engine.Globals.Clear();
            var autocomplete = engine.GetCompletionAt(source, 3).ToArray();
            var available = new String[0];

            CollectionAssert.AreEquivalent(available, autocomplete);
        }
    }
}
