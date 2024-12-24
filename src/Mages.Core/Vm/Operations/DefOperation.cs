namespace Mages.Core.Vm.Operations;

using System;

/// <summary>
/// Peeks the top element from the stack.
/// </summary>
sealed class DefOperation(String name) : IOperation
{
    private readonly String _name = name;

    public String Name => _name;

    public void Invoke(IExecutionContext context)
    {
        var value = context.Pop();

        if (context.Scope.ContainsKey(_name))
        {
            context.Scope[_name] = value;
        }
        else
        {
            context.Scope.Add(_name, value);
        }

        context.Push(value);
    }

    public override String ToString()
    {
        return String.Concat("def ", _name);
    }
}
