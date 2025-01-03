﻿namespace Mages.Core.Vm.Operations;

using System;
using System.Collections.Generic;

/// <summary>
/// Pops one value and tries to remove the key from the object.
/// Pushes the result on the stack.
/// </summary>
sealed class DelKeyOperation(String name) : IOperation
{
    private readonly String _name = name;

    public void Invoke(IExecutionContext context)
    {
        var obj = context.Pop() as IDictionary<String, Object>;
        var result = false;

        if (obj is not null)
        {
            result = obj.Remove(_name);
        }

        context.Push(result);
    }

    public override String ToString()
    {
        return "delkey " + _name;
    }
}
