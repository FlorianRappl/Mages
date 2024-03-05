namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Peeks the top element from the stack.
    /// </summary>
    sealed class InitMatOperation(Int32 row, Int32 col) : IOperation
    {
        private readonly Int32 _row = row;
        private readonly Int32 _col = col;

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            var matrix = (Object[,])context.Pop();
            matrix.SetValue(_row, _col, value);
            context.Push(matrix);
        }

        public override String ToString()
        {
            return String.Concat("initmat ", _row.ToString(), " ", _col.ToString());
        }
    }
}
