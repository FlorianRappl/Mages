namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pushes a new function on the stack.
    /// </summary>
    sealed class NewFuncOperation : IOperation
    {
        private readonly IOperation[] _operations;

        public NewFuncOperation(IOperation[] operations)
        {
            _operations = operations;
        }

        public void Invoke(IExecutionContext context)
        {
            var parentScope = context.Scope;
            var function = new InternalFunction(parentScope, _operations);
            context.Push(new Function(function.Invoke));
        }

        sealed class InternalFunction
        {
            private readonly IDictionary<String, Object> _parentScope;
            private readonly IOperation[] _operations;

            public InternalFunction(IDictionary<String, Object> parentScope, IOperation[] operations)
            {
                _parentScope = parentScope;
                _operations = operations;
            }

            public Object Invoke(Object[] arguments)
            {
                var scope = new LocalScope(_parentScope);
                var ctx = new ExecutionContext(_operations, scope);
                ctx.Push(arguments);
                ctx.Execute();
                return ctx.Pop();
            }
        }
    }
}
