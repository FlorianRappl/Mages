namespace Mages.Core.Vm
{
    using System;
    using System.Collections.Generic;

    sealed class SimpleMemory : IMemory
    {
        private readonly Dictionary<Int32, Object> _values;

        public SimpleMemory()
        {
            _values = new Dictionary<Int32, Object>();
        }

        public Object Load(Int32 address)
        {
            var value = default(Object);
            _values.TryGetValue(address, out value);
            return value;
        }

        public void Store(Int32 address, Object value)
        {
            _values[address] = value;
        }

        public void Clear()
        {
            _values.Clear();
        }
    }
}
