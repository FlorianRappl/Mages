namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime.Functions;
    using System;

    /// <summary>
    /// Takes two elements from the stack and pushes one.
    /// </summary>
    sealed class GetcOperation(Int32 length) : IOperation
    {
        private readonly Int32 _length = length;

        public void Invoke(IExecutionContext context)
        {
            var result = default(Object);
            var obj = context.Pop();

            if (obj is Function function || TypeFunctions.TryFind(obj, out function))
            {
                // We can call something, so let's prepare arguments

                if (_length == 0)
                {
                    result = function.Invoke([]);
                }
                else
                {
                    var arguments = new Object[_length];

                    for (var i = 0; i < _length; i++)
                    {
                        arguments[i] = context.Pop();
                    }

                    result = function.Invoke(arguments);
                }
            }
            else
            {
                // Nothing to call, we still need to pop the pushed arguments
                for (var i = 0; i < _length; i++)
                {
                    context.Pop();
                }
            }

            context.Push(result);
        }

        public override String ToString() => String.Concat("getc ", _length.ToString());
    }
}
