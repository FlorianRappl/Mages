namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Takes two values from the stack.
    /// </summary>
    sealed class InitObjOperation : IOperation
    {
        public static readonly IOperation Instance = new InitObjOperation();

        private InitObjOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            var name = (String)context.Pop();
            var obj = (IDictionary<String, Object>)context.Pop();
            obj.SetProperty(name, value);
            context.Push(obj);
        }
    }
}
