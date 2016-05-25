namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Stores the top-most element on the stack.
    /// </summary>
    sealed class StoreOperation : IOperation
    {
        private readonly String _name;

        public StoreOperation(String name)
        {
            _name = name;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            context.Scope.SetProperty(_name, value);
            context.Push(value);
        }
    }
}
