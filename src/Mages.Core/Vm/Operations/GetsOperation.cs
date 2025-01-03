﻿namespace Mages.Core.Vm.Operations;

using Mages.Core.Runtime;
using System;

/// <summary>
/// Pushes the loaded value on the stack.
/// </summary>
sealed class GetsOperation(String name) : IOperation
{
    private readonly String _name = name;

    public String Name => _name;

    public void Invoke(IExecutionContext context)
    {
        var value = context.Scope.GetProperty(_name);
        context.Push(value);
    }

    public override String ToString()
    {
        return String.Concat("gets ", _name);
    }
}
