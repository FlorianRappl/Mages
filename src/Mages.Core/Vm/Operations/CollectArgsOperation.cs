namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Takes some objects from the stack and returns one.
    /// </summary>
    sealed class CollectArgsOperation : IOperation
    {
        private readonly IMagesType[] _arguments;

        public CollectArgsOperation(Int32 length)
        {
            _arguments = new IMagesType[length];
        }

        public void Invoke(IExecutionContext context)
        {
            for (var i = 0; i < _arguments.Length; i++)
            {
                _arguments[i] = (IMagesType)context.Pop();
            }

            context.Push(_arguments);
        }
    }
}
