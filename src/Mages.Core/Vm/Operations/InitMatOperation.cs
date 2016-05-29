namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    sealed class InitMatOperation : IOperation
    {
        private readonly Int32 _row;
        private readonly Int32 _col;

        public InitMatOperation(Int32 row, Int32 col)
        {
            _row = row;
            _col = col;
        }

        public void Invoke(IExecutionContext context)
        {
            var value = (Double)context.Pop();
            var matrix = (Double[,])context.Pop();
            matrix.SetValue(_row, _col, value);
            context.Push(matrix);
        }
    }
}
