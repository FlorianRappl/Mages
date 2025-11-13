using Mages.Core.Tokens;
using NUnit.Framework;

namespace Mages.Core.Tests;

[TestFixture]
public class NumberHelperTests
{
    [TestCase(0u, 0u)]
    [TestCase(1u, 0u)]
    [TestCase(0u, 1u)]
    [TestCase(1u, 1u)]
    [TestCase(123u, 123u)]
    [TestCase(20u, 123u)]
    [TestCase(10u, ulong.MaxValue / 10)]
    [TestCase(0u, ulong.MaxValue)]
    public void TryMultiply_happy_case(ulong x, ulong y)
    {
        var result = NumberHelper.TryMultiply(x, y, out var actual);
        Assert.AreEqual(checked(x * y), actual);
        Assert.IsTrue(result);
    }

    [TestCase(ulong.MaxValue, 2u)]
    [TestCase(ulong.MaxValue, ulong.MaxValue)]
    [TestCase(ulong.MaxValue / 2 + 1, 3u)]
    [TestCase(ulong.MaxValue / 3 + 1, 4u)]
    [TestCase(ulong.MaxValue - 1, 2u)]
    [TestCase(ulong.MaxValue / 10 * 9 + 1, 2u)]
    [TestCase(0xFFFFFFFFFFFFFFFFu, 2u)]
    [TestCase(8409014139716477191u, 10u)]
    public void TryMultiply_overflow_case(ulong x, ulong y)
    {
        var result = NumberHelper.TryMultiply(x, y, out var actual);
        Assert.AreEqual(0, actual);
        Assert.IsFalse(result);
    }

    [TestCase(0u, 0u)]
    [TestCase(1u, 0u)]
    [TestCase(0u, 1u)]
    [TestCase(1u, 1u)]
    [TestCase(123u, 123u)]
    [TestCase(20u, 123u)]
    [TestCase(0u, ulong.MaxValue)]
    [TestCase(ulong.MaxValue, 0u)]
    [TestCase(ulong.MaxValue - 10, 10u)]
    public void TryAdd_happy_case(ulong x, ulong y)
    {
        var result = NumberHelper.TryAdd(x, y, out var actual);
        Assert.AreEqual(checked(x + y), actual);
        Assert.IsTrue(result);
    }

    [TestCase(1u, ulong.MaxValue)]
    [TestCase(ulong.MaxValue, 1u)]
    [TestCase(ulong.MaxValue, ulong.MaxValue)]
    [TestCase(ulong.MaxValue / 2 + 1, ulong.MaxValue / 2 + 2)]
    [TestCase(ulong.MaxValue - 1, 2u)]
    [TestCase(0xFFFFFFFFFFFFFFFFu, 1u)]
    public void TryAdd_overflow_case(ulong x, ulong y)
    {
        var result = NumberHelper.TryAdd(x, y, out var actual);
        Assert.AreEqual(0, actual);
        Assert.IsFalse(result);
    }
}