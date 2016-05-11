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
        private readonly Stack<IMagesType> _stack;
        private readonly IOperation[] _operations;
        private Int32 _position;

        public ExecutionContext(IOperation[] operations)
        {
            _stack = new Stack<IMagesType>();
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

        public void Push(IMagesType value)
        {
            _stack.Push(value);
        }

        public IMagesType Pop()
        {
            return _stack.Count > 0 ? _stack.Pop() : new Undefined();
        }
    }
}
