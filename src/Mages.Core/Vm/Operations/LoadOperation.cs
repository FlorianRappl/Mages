namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Types;
    using System;

    /// <summary>
    /// Loads a value by placing it on the stack.
    /// </summary>
    sealed class LoadOperation : IOperation
    {
        private readonly Func<IExecutionContext, IMagesType> _loader;

        public LoadOperation(Func<IExecutionContext, IMagesType> loader)
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
