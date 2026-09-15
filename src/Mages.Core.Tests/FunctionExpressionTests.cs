using Mages.Core.Ast.Expressions;
using Mages.Core.Ast.Statements;
using NUnit.Framework;

namespace Mages.Core.Tests;

[TestFixture]
public class FunctionExpressionTests
{
    [Test]
    public void ParseRecursiveFibonacciFunction()
    {
        var result = "var fib = n => n < 2 ? n : fib(n - 1) + fib(n - 2);".ToStatement();

        Assert.IsInstanceOf<VarStatement>(result);

        var assignment = (AssignmentExpression)((VarStatement)result).Assignment;

        Assert.IsInstanceOf<VariableExpression>(assignment.Variable);
        Assert.AreEqual("fib", ((VariableExpression)assignment.Variable).Name);
        Assert.IsInstanceOf<FunctionExpression>(assignment.Value);

        var fx = (FunctionExpression)assignment.Value;

        Assert.AreEqual(1, fx.Parameters.Parameters.Length);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[0]);
        Assert.AreEqual("n", ((VariableExpression)fx.Parameters.Parameters[0]).Name);

        Assert.IsInstanceOf<SimpleStatement>(fx.Body);

        var body = (SimpleStatement)fx.Body;

        Assert.IsInstanceOf<ConditionalExpression>(body.Expression);

        var condition = (ConditionalExpression)body.Expression;

        // n < 2
        Assert.IsInstanceOf<BinaryExpression.Less>(condition.Condition);

        var less = (BinaryExpression)condition.Condition;
        Assert.IsInstanceOf<VariableExpression>(less.LValue);
        Assert.AreEqual("n", ((VariableExpression)less.LValue).Name);
        Assert.IsInstanceOf<ConstantExpression>(less.RValue);
        Assert.AreEqual(2.0, (double)((ConstantExpression)less.RValue).Value);

        // n
        Assert.IsInstanceOf<VariableExpression>(condition.Primary);
        Assert.AreEqual("n", ((VariableExpression)condition.Primary).Name);

        // fib(n - 1) + fib(n - 2)
        Assert.IsInstanceOf<BinaryExpression.Add>(condition.Secondary);

        var add = (BinaryExpression)condition.Secondary;

        Assert.IsInstanceOf<CallExpression>(add.LValue);
        Assert.IsInstanceOf<CallExpression>(add.RValue);

        var firstCall = (CallExpression)add.LValue;
        Assert.IsInstanceOf<VariableExpression>(firstCall.Function);
        Assert.AreEqual("fib", ((VariableExpression)firstCall.Function).Name);
        Assert.AreEqual(1, firstCall.Arguments.Arguments.Length);
        Assert.IsInstanceOf<BinaryExpression.Subtract>(firstCall.Arguments.Arguments[0]);

        var secondCall = (CallExpression)add.RValue;
        Assert.IsInstanceOf<VariableExpression>(secondCall.Function);
        Assert.AreEqual("fib", ((VariableExpression)secondCall.Function).Name);
        Assert.AreEqual(1, secondCall.Arguments.Arguments.Length);
        Assert.IsInstanceOf<BinaryExpression.Subtract>(secondCall.Arguments.Arguments[0]);
    }

    [Test]
    public void ParseSimpleFunction()
    {
        var result = "()=>new{}".ToExpression();
        
        Assert.IsInstanceOf<FunctionExpression>(result);

        var fx = (FunctionExpression)result;
        Assert.AreEqual(0, fx.Parameters.Parameters.Length);

        Assert.IsInstanceOf<SimpleStatement>(fx.Body);

        var body = (SimpleStatement)fx.Body;

        Assert.IsInstanceOf<ObjectExpression>(body.Expression);
    }

    [Test]
    public void ParseSimpleFunctionWithImplicitReturn()
    {
        var result = "()=>2*3".ToExpression();

        Assert.IsInstanceOf<FunctionExpression>(result);

        var fx = (FunctionExpression)result;

        Assert.AreEqual(0, fx.Parameters.Parameters.Length);

        Assert.IsInstanceOf<SimpleStatement>(fx.Body);

        var body = (SimpleStatement)fx.Body;
        Assert.IsInstanceOf<BinaryExpression.Multiply>(body.Expression);

        var multiply = (BinaryExpression)body.Expression;

        Assert.IsInstanceOf<ConstantExpression>(multiply.LValue);
        Assert.IsInstanceOf<ConstantExpression>(multiply.RValue);
    }

    [Test]
    public void ParseSimpleFunctionWithOneArgument()
    {
        var result = "(x) => new{}".ToExpression();

        Assert.IsInstanceOf<FunctionExpression>(result);

        var fx = (FunctionExpression)result;
        Assert.AreEqual(1, fx.Parameters.Parameters.Length);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[0]);

        var x = (VariableExpression)fx.Parameters.Parameters[0];
        Assert.AreEqual("x", x.Name);

        Assert.IsInstanceOf<SimpleStatement>(fx.Body);

        var body = (SimpleStatement)fx.Body;
        Assert.IsInstanceOf<ObjectExpression>(body.Expression);
    }

    [Test]
    public void ParseSimpleFunctionWithTwoArguments()
    {
        var result = "(x,y)=>new {}".ToExpression();

        Assert.IsInstanceOf<FunctionExpression>(result);

        var fx = (FunctionExpression)result;
        Assert.AreEqual(2, fx.Parameters.Parameters.Length);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[0]);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[1]);

        var x = (VariableExpression)fx.Parameters.Parameters[0];
        Assert.AreEqual("x", x.Name);
        var y = (VariableExpression)fx.Parameters.Parameters[1];
        Assert.AreEqual("y", y.Name);

        Assert.IsInstanceOf<SimpleStatement>(fx.Body);

        var body = (SimpleStatement)fx.Body;
        Assert.IsInstanceOf<ObjectExpression>(body.Expression);
    }

    [Test]
    public void ParseSimpleFunctionWithThreeArguments()
    {
        var result = "(x,y, abc)=>new{}".ToExpression();

        Assert.IsInstanceOf<FunctionExpression>(result);

        var fx = (FunctionExpression)result;
        Assert.AreEqual(3, fx.Parameters.Parameters.Length);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[0]);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[1]);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[2]);

        var x = (VariableExpression)fx.Parameters.Parameters[0];
        Assert.AreEqual("x", x.Name);
        var y = (VariableExpression)fx.Parameters.Parameters[1];
        Assert.AreEqual("y", y.Name);
        var abc = (VariableExpression)fx.Parameters.Parameters[2];
        Assert.AreEqual("abc", abc.Name);

        Assert.IsInstanceOf<SimpleStatement>(fx.Body);

        var body = (SimpleStatement)fx.Body;
        Assert.IsInstanceOf<ObjectExpression>(body.Expression);
    }

    [Test]
    public void ParseSimpleFunctionWithSingleNakedArgument()
    {
        var result = "_=>new{}".ToExpression();

        Assert.IsInstanceOf<FunctionExpression>(result);

        var fx = (FunctionExpression)result;
        Assert.AreEqual(1, fx.Parameters.Parameters.Length);
        Assert.IsInstanceOf<VariableExpression>(fx.Parameters.Parameters[0]);

        var underscore = (VariableExpression)fx.Parameters.Parameters[0];
        Assert.AreEqual("_", underscore.Name);

        Assert.IsInstanceOf<SimpleStatement>(fx.Body);

        var body = (SimpleStatement)fx.Body;
        Assert.IsInstanceOf<ObjectExpression>(body.Expression);
    }
}
