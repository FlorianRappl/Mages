namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime.Functions;
    using System;

    /// <summary>
    /// Populates the local scope with one of the arguments.
    /// </summary>
    sealed class ArgOperation(Int32 index, String name) : IOperation
    {
        private readonly Int32 _index = index;
        private readonly String _name = name;

        public void Invoke(IExecutionContext context)
        {
            var parameters = (Object[])context.Pop();

            if (parameters.Length == 0)
            {
                context.Pause();
            }
            else if (parameters.Length == _index)
            {
                var function = (Function)context.Pop();
                var value = Curry.Min(_index + 1, function, parameters);
                context.Push(value);
                context.Pause();
            }
            else
            {
                var value = parameters[_index];
                context.Scope.Add(_name, value);
                context.Push(parameters);
            }
        }

        public override String ToString()
        {
            return String.Concat("arg ", _index.ToString(), " ", _name);
        }
    }
}
