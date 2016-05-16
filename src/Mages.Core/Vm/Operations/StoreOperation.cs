namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Stores a value by placing it on the stack.
    /// </summary>
    sealed class StoreOperation : IOperation
    {
        private readonly Action<IExecutionContext, Object> _store;

        public StoreOperation(Action<IExecutionContext, Object> store)
        {
            _store = store;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            _store.Invoke(context, value);
            context.Push(value);
        }
    }
}
