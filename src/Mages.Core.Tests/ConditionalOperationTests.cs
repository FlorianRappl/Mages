using Mages.Core.Ast;
using Mages.Core.Vm;
using Mages.Core.Vm.Operations;
using NUnit.Framework;
using System.Collections.Generic;

namespace Mages.Core.Tests;

[TestFixture]
public class ConditionalOperationTests
{
    [Test]
    public void ConditionalExpressionGeneratesShortCircuitOperations()
    {
        var statement = "true ? 1 : 2".ToStatement();
        var operations = new List<IStatement> { statement }.MakeRunnable();

        // condition, popif, jump-to-secondary, primary, jump-to-end, secondary
        Assert.AreEqual(6, operations.Length);
        Assert.IsInstanceOf<ConstOperation>(operations[0]);
        Assert.IsInstanceOf<PopIfOperation>(operations[1]);
        Assert.IsInstanceOf<JumpOperation>(operations[2]);
        Assert.IsInstanceOf<ConstOperation>(operations[3]);
        Assert.IsInstanceOf<JumpOperation>(operations[4]);
        Assert.IsInstanceOf<ConstOperation>(operations[5]);
    }

    [Test]
    public void ConditionalExpressionOnlyExecutesTheSelectedBranch()
    {
        var statement = "true ? 1 : 2".ToStatement();
        var operations = new List<IStatement> { statement }.MakeRunnable();
        var scope = new Dictionary<string, object>();
        var context = new ExecutionContext(operations, scope);

        context.Execute();

        Assert.AreEqual(1.0, context.Pop());
    }

    [Test]
    public void ConditionalExpressionSkipsTheOtherBranchWhenFalse()
    {
        var statement = "false ? 1 : 2".ToStatement();
        var operations = new List<IStatement> { statement }.MakeRunnable();
        var scope = new Dictionary<string, object>();
        var context = new ExecutionContext(operations, scope);

        context.Execute();

        Assert.AreEqual(2.0, context.Pop());
    }
}
