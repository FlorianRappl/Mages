namespace Mages.Core.Runtime.Functions
{
    using System;

    sealed class ArithmeticFunction : StandardFunction
    {
        private readonly Func<Double, Double> _function;

        public ArithmeticFunction(Func<Double, Double> function)
        {
            _function = function;
        }

        public override Object Invoke(Double value)
        {
            return _function.Invoke(value);
        }
    }
}
