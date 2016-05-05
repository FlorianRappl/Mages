namespace Mages.Core.Vm
{
    using Mages.Core.Types;
    using System;
    using System.Collections.Generic;

    sealed class SimpleMemory : IMemory
    {
        private readonly Dictionary<Int32, IMagesType> _values;

        public SimpleMemory()
        {
            _values = new Dictionary<Int32, IMagesType>();
        }

        public IMagesType Load(Int32 address)
        {
            var value = default(IMagesType);
            _values.TryGetValue(address, out value);
            return value;
        }

        public void Store(Int32 address, IMagesType value)
        {
            _values[address] = value;
        }

        public void Clear()
        {
            _values.Clear();
        }
    }
}
