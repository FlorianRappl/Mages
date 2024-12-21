namespace Mages.Core.Vm.Operations;

using System;

/// <summary>
/// Tries to remove the named variable from the scope and pushes the result
/// on the stack.
/// </summary>
sealed class DelVarOperation(String name) : IOperation
{
    private readonly String _name = name;

    public String Name => _name;

    public void Invoke(IExecutionContext context)
    {
        var result = context.Scope.Remove(_name);
        context.Push(result);
    }

    public override String ToString()
    {
        return "delvar " + _name;
    }
}
