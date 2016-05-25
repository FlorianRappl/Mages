namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Loads a value by placing it on the stack.
    /// </summary>
    sealed class LoadOperation : IOperation
    {
        private readonly String _name;

        public LoadOperation(String name)
        {
            _name = name;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Scope.GetProperty(_name);
            context.Push(value);
        }
    }
}
