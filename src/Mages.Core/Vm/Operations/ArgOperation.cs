namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Populates the local scope with one of the arguments.
    /// </summary>
    sealed class ArgOperation : IOperation
    {
        private readonly Int32 _index;
        private readonly String _name;

        public ArgOperation(Int32 index, String name)
        {
            _index = index;
            _name = name;
        }

        public void Invoke(IExecutionContext context)
        {
            var parameters = (Object[])context.Pop();
            var value = parameters.Length > _index ? parameters[_index] : null;
            context.Scope.Add(_name, value);
            context.Push(parameters);
        }
    }
}
