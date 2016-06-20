namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Stores the top-most element on the stack.
    /// </summary>
    sealed class DefOperation : IOperation
    {
        private readonly String _name;

        public DefOperation(String name)
        {
            _name = name;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();

            if (context.Scope.ContainsKey(_name))
            {
                context.Scope[_name] = value;
            }
            else
            {
                context.Scope.Add(_name, value);
            }

            context.Push(value);
        }

        public override String ToString()
        {
            return String.Concat("def ", _name);
        }
    }
}
