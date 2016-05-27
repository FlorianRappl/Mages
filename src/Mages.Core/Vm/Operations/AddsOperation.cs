namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Stores the top-most element on the stack.
    /// </summary>
    sealed class AddsOperation : IOperation
    {
        private readonly String _name;

        public AddsOperation(String name)
        {
            _name = name;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            context.Scope.Add(_name, value);
            context.Push(value);
        }
    }
}
