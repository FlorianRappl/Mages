namespace Mages.Core.Runtime.Functions
{
    using Mages.Core.Runtime.Converters;
    using System;
    using System.Collections.Generic;

    sealed class ArrayFunction : StandardFunction
    {
        private readonly Func<IEnumerable<Double>, IEnumerable<Double>> _manip;

        public ArrayFunction(Func<IEnumerable<Double>, IEnumerable<Double>> manip)
        {
            _manip = manip;
        }

        public override Object Invoke(Double value)
        {
            return value;
        }

        public override Object Invoke(Double[,] matrix)
        {
            var source = matrix.ToVector();
            return _manip.Invoke(source).ToMatrix();
        }
    }
}
