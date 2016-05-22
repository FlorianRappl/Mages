namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Takes one object from the stack and increments it.
    /// </summary>
    sealed class DecOperation : IOperation
    {
        private readonly IOperation _store;
        private readonly Boolean _postOperation;
        private readonly Object[] _arguments;

        public DecOperation(IOperation store, Boolean postOperation)
        {
            _store = store;
            _postOperation = postOperation;
            _arguments = new Object[2];
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            _arguments[0] = value;
            _arguments[1] = 1.0;
            context.Push(BinaryOperators.Sub(_arguments));
            _store.Invoke(context);

            if (_postOperation)
            {
                context.Pop();
                context.Push(value);
            }
        }
    }
}
