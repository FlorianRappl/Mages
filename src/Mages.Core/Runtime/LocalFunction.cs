namespace Mages.Core.Runtime
{
    using Mages.Core.Vm;
    using System;
    using System.Collections.Generic;

    public class LocalFunction
    {
        private readonly Object _self;
        private readonly IDictionary<String, Object> _parentScope;
        private readonly String[] _parameters;
        private readonly IOperation[] _operations;
        private readonly Function _pointer;

        public LocalFunction(IDictionary<String, Object> parentScope, String[] parameters, IOperation[] operations)
            : this(null, parentScope, parameters, operations)
        {
        }

        public LocalFunction(Object self, IDictionary<String, Object> parentScope, String[] parameters, IOperation[] operations)
        {
            _self = self;
            _parentScope = parentScope;
            _parameters = parameters;
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

            if (_self != null)
            {
                scope.Add("this", _self);
            }

            ctx.Push(_pointer);
            ctx.Push(arguments);
            ctx.Execute();
            return ctx.Pop();
        }
    }
}
