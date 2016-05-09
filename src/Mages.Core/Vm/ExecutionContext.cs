namespace Mages.Core.Vm
{
    using Mages.Core.Types;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Assembles the MAGES runtime memory model.
    /// </summary>
    public sealed class ExecutionContext : IExecutionContext
    {
        private readonly Stack<Object> _stack;
        private readonly IOperation[] _operations;
        private Int32 _position;

        public ExecutionContext(IOperation[] operations)
        {
            _stack = new Stack<Object>();
            _operations = operations;
            _position = 0;
        }

        public Int32 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public void Execute(IDictionary<String, IMagesType> globalScope)
        {
            while (_position < _operations.Length)
            {
                _operations[_position].Invoke(this);
                _position++;
            }
        }

        public void Push(Object value)
        {
            _stack.Push(value);
        }

        public Object Pop()
        {
            return _stack.Count > 0 ? _stack.Pop() : new Undefined();
        }
    }
}
