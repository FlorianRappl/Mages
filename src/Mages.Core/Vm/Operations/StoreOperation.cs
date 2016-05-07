namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Types;

    /// <summary>
    /// Stores a value by placing it on the stack.
    /// </summary>
    sealed class StoreOperation : IOperation
    {
        public void Invoke(IExecutionContext context)
        {
            var pointer = (Pointer)context.Pop();
            var value = (IMagesType)context.Pop();
            pointer.Reference = value;
        }
    }
}
