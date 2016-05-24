namespace Mages.Core.Runtime.Functions
{
    using System;

    sealed class ThrowFunction : StandardFunction
    {
        public override Object Invoke(Double[,] matrix)
        {
            return NotImplemented;
        }

        public override Object Invoke(String value)
        {
            throw new Exception(value);
        }
    }
}
