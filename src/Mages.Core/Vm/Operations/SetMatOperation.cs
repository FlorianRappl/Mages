namespace Mages.Core.Vm.Operations;

using Mages.Core.Runtime;
using Mages.Core.Runtime.Converters;
using System;
using System.Numerics;

/// <summary>
/// Converts the generic matrix to a specific matrix.
/// </summary>
sealed class SetMatOperation : IOperation
{
    public SetMatOperation()
    {
    }

    public void Invoke(IExecutionContext context)
    {
        var matrix = (Object[,])context.Pop();
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        var count = matrix.CountAll(m => m is Complex);
        var isComplex = count > 0;

        if (isComplex)
        {
            var result = matrix.ForEach(z => z.ToComplex());
            context.Push(result);
        }
        else
        {
            var result = matrix.ForEach(z => z.ToNumber());
            context.Push(result);
        }
    }

    public override String ToString()
    {
        return String.Concat("setmat");
    }
}
