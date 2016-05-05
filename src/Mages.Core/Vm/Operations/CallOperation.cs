namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Types;

    /// <summary>
    /// Takes two objects from the stack and returns one.
    /// </summary>
    sealed class CallOperation : IOperation
    {
        public void Invoke(IExecutionContext context)
        {
            var function = (Function)context.Pop();
            var arguments = (IMagesType[])context.Pop();
            context.Push(function.Invoke(arguments));
        }
    }
}
