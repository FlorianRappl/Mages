namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Stores a value by placing it on the stack.
    /// </summary>
    sealed class StoreOperation : IOperation
    {
        private readonly String _variableName;

        public StoreOperation(String variableName)
        {
            _variableName = variableName;
        }

        public void Invoke(IExecutionContext context)
        {
            //var pointer = (Pointer)context.Pop();
            //var value = (IMagesType)context.Pop();
            //pointer.Reference = value;
        }
    }
}
