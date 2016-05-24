namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Collections.Generic;

    sealed class CatchFunction : StandardFunction
    {
        private static readonly Object[] Empty = new Object[0];

        public override Object Invoke(Double[,] matrix)
        {
            return NotImplemented;
        }

        public override Object Invoke(Function function)
        {
            var result = new Dictionary<String, Object>
            {
                { "value", null },
                { "error", null }
            };
            
            try
            {
                result["value"] = function.Invoke(Empty);
            }
            catch (Exception ex)
            {
                result["error"] = ex.Message;
            }

            return result;
        }
    }
}
