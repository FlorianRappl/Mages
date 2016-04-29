namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Loads a constant by placing it on the stack.
    /// </summary>
    sealed class LoadOperation : IOperation
    {
        private readonly Object _value;

        public LoadOperation(Object value)
        {
            _value = value;
        }

        public void Invoke(IExecutionContext context)
        {
            context.Push(_value);
        }
    }
}
