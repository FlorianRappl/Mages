namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime.Converters;

    sealed class CondOperation : IOperation
    {
        public static readonly IOperation Instance = new CondOperation();

        private CondOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            var condition = context.Pop().ToBoolean();
            var primary = context.Pop();
            var alternative = context.Pop();
            context.Push(condition ? primary : alternative);
        }
    }
}
