﻿namespace Mages.Core.Vm.Operations;

using System;

/// <summary>
/// Pushes one new element on the stack.
/// </summary>
sealed class NewMatOperation(Int32 rows, Int32 cols) : IOperation
{
    private readonly Int32 _rows = rows;
    private readonly Int32 _cols = cols;

    public void Invoke(IExecutionContext context)
    {
        context.Push(new Object[_rows, _cols]);
    }

    public override String ToString()
    {
        return String.Concat("newmat ", _rows.ToString(), " ", _cols.ToString());
    }
}
