namespace Mages.Core.Vm.Operations;

using System;

/// <summary>
/// Pushes a constant value on the stack.
/// </summary>
sealed class ConstOperation(Object constant) : IOperation
{
    private readonly Object _constant = constant;

    /// <summary>
    /// Contains a const operation pushing null on the stack.
    /// </summary>
    public static readonly IOperation Null = new ConstOperation(null);

    public void Invoke(IExecutionContext context)
    {
        context.Push(_constant);
    }

    public override String ToString()
    {
        return String.Concat("const ", _constant != null ? _constant.GetHashCode() : 0);
    }
}
