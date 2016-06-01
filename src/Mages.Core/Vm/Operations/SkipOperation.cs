namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime.Converters;

    /// <summary>
    /// Takes one value from the stack and potentially changes the position.
    /// </summary>
    sealed class SkipOperation : IOperation
    {
        public static readonly IOperation Instance = new SkipOperation();

        private SkipOperation()
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
    }
}
