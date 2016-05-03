namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Loads a constant by placing it on the stack.
    /// </summary>
    sealed class LoadOperation : IOperation
    {
        private readonly Func<IExecutionContext, Object> _loader;

        public LoadOperation(Func<IExecutionContext, Object> loader)
        {
            _loader = loader;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = _loader.Invoke(context);
            context.Push(value);
        }
    }
}
