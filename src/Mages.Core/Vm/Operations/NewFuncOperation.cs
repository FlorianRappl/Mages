namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pushes one new element on the stack.
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
            context.Push(function.Pointer);
        }

        public override String ToString()
        {
            var instructions = new String[3];
            instructions[0] = "newfunc start";
            instructions[1] = _operations.Serialize();
            instructions[2] = "newfunc end";
            return String.Join(Environment.NewLine, instructions);
        }

        sealed class InternalFunction
        {
            private readonly IDictionary<String, Object> _parentScope;
            private readonly IOperation[] _operations;
            private readonly Function _pointer;

            public InternalFunction(IDictionary<String, Object> parentScope, IOperation[] operations)
            {
                _parentScope = parentScope;
                _operations = operations;
                _pointer = new Function(Invoke);
            }

            public Function Pointer
            {
                get { return _pointer; }
            }

            public Object Invoke(Object[] arguments)
            {
                var scope = new LocalScope(_parentScope);
                var ctx = new ExecutionContext(_operations, scope);
                ctx.Push(_pointer);
                ctx.Push(arguments);
                ctx.Execute();
                return ctx.Pop();
            }
        }
    }
}
