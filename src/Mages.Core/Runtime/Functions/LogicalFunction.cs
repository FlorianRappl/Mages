namespace Mages.Core.Runtime.Functions
{
    using Mages.Core.Runtime.Converters;
    using System;

    sealed class LogicalFunction : StandardFunction
    {
        private readonly Func<Double, Boolean> _function;

        public LogicalFunction(Func<Double, Boolean> function)
        {
            _function = function;
        }

        public override Object Invoke(Double value)
        {
            return _function.Invoke(value);
        }

        protected override Double Compute(Double value)
        {
            return _function.Invoke(value).ToNumber();
        }
    }
}
