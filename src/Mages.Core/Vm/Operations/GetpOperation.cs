namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Takes two values from the stack and pushes the result.
    /// </summary>
    sealed class GetpOperation : IOperation
    {
        public static readonly IOperation Instance = new GetpOperation();

        private GetpOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            var obj = context.Pop() as IDictionary<String, Object>;
            var name = context.Pop() as String;
            var result = default(Object);

            if (obj != null && name != null)
            {
                result = obj.GetProperty(name);
            }
            
            context.Push(result);
        }
    }
}
