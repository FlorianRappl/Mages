namespace Mages.Core.Vm.Operations
{
    /// <summary>
    /// Takes one value from the stack without placing a new one.
    /// </summary>
    sealed class PopOperation : IOperation
    {
        public static readonly IOperation Instance = new PopOperation();

        private PopOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            context.Pop();
        }
    }
}
