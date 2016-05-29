namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using Mages.Core.Runtime.Converters;
    using System;

    sealed class RangeOperation : IOperation
    {
        private readonly Boolean _hasStep;

        public RangeOperation(Boolean hasStep)
        {
            _hasStep = hasStep;
        }

        public void Invoke(IExecutionContext context)
        {
            var from = context.Pop().ToNumber();
            var to = context.Pop().ToNumber();
            var result = default(Object);

            if (_hasStep)
            {
                var step = context.Pop().ToNumber();
                result = Range.Create(from, to, step);
            }
            else
            {
                result = Range.Create(from, to);
            }

            context.Push(result);
        }
    }
}
