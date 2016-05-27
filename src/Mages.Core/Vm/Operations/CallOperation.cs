namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Takes two objects from the stack and returns one.
    /// </summary>
    sealed class CallOperation : IOperation
    {
        private readonly Object[] _arguments;

        public CallOperation(Int32 length)
        {
            _arguments = new Object[length];
        }

        public void Invoke(IExecutionContext context)
        {
            var result = default(Object);
            var obj = context.Pop();

            for (var i = 0; i < _arguments.Length; i++)
            {
                _arguments[i] = context.Pop();
            }

            if (obj != null)
            {
                var function = obj as Function;

                if (function != null)
                {
                    result = function.Invoke(_arguments);
                }
            }

            context.Push(result);
        }
    }
}
