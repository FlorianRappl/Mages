namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Takes two objects from the stack and returns one.
    /// </summary>
    sealed class CallOperation : IOperation
    {
        public void Invoke(IExecutionContext context)
        {
            var function = context.Pop() as Func<Object[], Object>;
            var arguments = context.Pop() as Object[];
            context.Push(function.Invoke(arguments));
        }
    }
}
