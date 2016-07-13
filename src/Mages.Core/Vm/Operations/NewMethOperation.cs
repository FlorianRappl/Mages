namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pops two elements from the stack and pushes three new element on the stack.
    /// </summary>
    sealed class NewMethOperation : IOperation
    {
        private readonly IOperation[] _operations;

        public NewMethOperation(IOperation[] operations)
        {
            _operations = operations;
        }

        public void Invoke(IExecutionContext context)
        {
            var name = context.Pop();
            var obj = context.Pop();
            var parentScope = context.Scope;
            var function = new InternalMethod(obj, parentScope, _operations);
            context.Push(obj);
            context.Push(name);
            context.Push(function.Pointer);
        }

        public override String ToString()
        {
            var instructions = new String[3];
            instructions[0] = "newmeth start";
            instructions[1] = _operations.Serialize();
            instructions[2] = "newmeth end";
            return String.Join(Environment.NewLine, instructions);
        }

        sealed class InternalMethod
        {
            private readonly Object _self;
            private readonly Scope _parentScope;
            private readonly IOperation[] _operations;
            private readonly Function _pointer;

            public InternalMethod(Object self, Scope parentScope, IOperation[] operations)
            {
                _self = self;
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
                var scope = new Scope(_parentScope);
                var ctx = new ExecutionContext(_operations, scope);
                scope.Add("this", _self);
                ctx.Push(_pointer);
                ctx.Push(arguments);
                ctx.Execute();
                return ctx.Pop();
            }
        }
    }
}
