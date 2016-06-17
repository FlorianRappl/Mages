namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Takes one object from the stack and increments it.
    /// </summary>
    sealed class DecOperation : IOperation
    {
        public static readonly IOperation Instance = new DecOperation();

        private DecOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            var arguments = new Object[] { 1.0, value };
            context.Push(BinaryOperators.Sub(arguments));
        }
    }
}
