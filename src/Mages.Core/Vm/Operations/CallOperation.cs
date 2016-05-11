namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Types;
    using System;

    /// <summary>
    /// Takes two objects from the stack and returns one.
    /// </summary>
    sealed class CallOperation : IOperation
    {
        private readonly IMagesType[] _arguments;

        public CallOperation(Int32 length)
        {
            _arguments = new IMagesType[length];
        }

        public void Invoke(IExecutionContext context)
        {
            var function = (Function)context.Pop();

            for (var i = 0; i < _arguments.Length; i++)
            {
                _arguments[i] = context.Pop();
            }

            context.Push(function.Invoke(_arguments));
        }
    }
}
