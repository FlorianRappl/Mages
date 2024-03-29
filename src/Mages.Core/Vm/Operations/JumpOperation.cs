﻿namespace Mages.Core.Vm.Operations;

using System;

/// <summary>
/// Changes the currently executing position.
/// </summary>
sealed class JumpOperation(Int32 offset) : IOperation
{
    private readonly Int32 _offset = offset;

    public void Invoke(IExecutionContext context)
    {
        context.Position += _offset;
    }

    public override String ToString()
    {
        return String.Concat("jump ", _offset.ToString("+0;-0;0"));
    }
}
