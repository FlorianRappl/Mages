using NUnit.Framework;

namespace Mages.Core.Tests;

[TestFixture]
public class PreprocessorTests
{
    [Test]
    public void InitialSheBangIsIgnored()
    {
        var source = @"#!/bin/mages
2+3";
        var result = source.Eval();
        Assert.AreEqual(5.0, result);
    }

    [Test]
    public void TrailingPreprocessorIsTreatedLikeLineComment()
    {
        var source = @"2+3 # this is some comment";
        var result = source.Eval();
        Assert.AreEqual(5.0, result);
    }
}
