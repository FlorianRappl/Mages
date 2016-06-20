namespace Mages.Core.Vm.Operations
{
    using System;

    sealed class RetOperation : IOperation
    {
        public static readonly IOperation Instance = new RetOperation();

        private RetOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            context.Stop();
        }

        public override String ToString()
        {
            return "ret";
        }
    }
}
