namespace Mages.Core.Vm
{
    using System;

    struct Pointer
    {
        public IMemory Memory;
        public Int32 Address;

        public Object Value
        {
            get { return Memory.Load(Address); }
            set { Memory.Store(Address, value); }
        }
    }
}
