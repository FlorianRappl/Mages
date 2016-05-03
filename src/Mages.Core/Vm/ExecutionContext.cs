namespace Mages.Core.Vm
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Assembles the MAGES runtime memory model.
    /// </summary>
    public sealed class ExecutionContext : IExecutionContext
    {
        private readonly Stack<Object> _stack;
        private readonly IOperation[] _operations;
        private readonly IMemory _memory;
        private Int32 _position;

        public ExecutionContext(IOperation[] operations, IMemory memory)
        {
            _stack = new Stack<Object>();
            _operations = operations;
            _memory = memory;
            _position = 0;
        }

        public Int32 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public IMemory Memory
        {
            get { return _memory; }
        }

        public void Execute()
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
            return _stack.Pop();
        }
    }
}
