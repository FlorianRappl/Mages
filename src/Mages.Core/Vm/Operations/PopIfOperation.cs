namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime.Converters;
    using System;

    /// <summary>
    /// Takes one value from the stack and potentially changes the position.
    /// </summary>
    sealed class PopIfOperation : IOperation
    {
        public static readonly IOperation Instance = new PopIfOperation();

        private PopIfOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            var shouldSkip = context.Pop().ToBoolean();

            if (shouldSkip)
            {
                context.Position++;
            }
        }

        public override String ToString()
        {
            return "popif";
        }
    }
}
