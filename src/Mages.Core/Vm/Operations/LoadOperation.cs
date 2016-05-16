namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Loads a value by placing it on the stack.
    /// </summary>
    sealed class LoadOperation : IOperation
    {
        private readonly Func<IExecutionContext, Object> _load;

        public LoadOperation(Func<IExecutionContext, Object> load)
        {
            _load = load;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = _load.Invoke(context);
            context.Push(value);
        }
    }
}
