namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Collections.Generic;

    sealed class LengthFunction : StandardFunction
    {
        public override Object Invoke(IDictionary<String, Object> obj)
        {
            return (Double)obj.Count;
        }

        public override Object Invoke(String value)
        {
            return (Double)value.Length;
        }

        public override Object Invoke(Double value)
        {
            return 1.0;
        }

        public override Object Invoke(Double[,] matrix)
        {
            return (Double)(matrix.GetCount());
        }

        public override Object Invoke(Function function)
        {
            return 1.0;
        }
    }
}
