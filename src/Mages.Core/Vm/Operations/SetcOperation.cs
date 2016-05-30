namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime.Functions;
    using System;

    /// <summary>
    /// Takes three objects from the stack and returns one.
    /// </summary>
    sealed class SetcOperation : IOperation
    {
        private readonly Int32 _length;

        public SetcOperation(Int32 length)
        {
            _length = length;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            var obj = context.Pop();
            var function = default(Procedure);
            var arguments = new Object[_length];

            for (var i = 0; i < arguments.Length; i++)
            {
                arguments[i] = context.Pop();
            }

            if (obj != null && TypeFunctions.TryFindSetter(obj, out function))
            {
                function.Invoke(arguments, value);
            }

            context.Push(value);
        }
    }
}
