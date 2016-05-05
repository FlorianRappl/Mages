namespace Mages.Core.Types
{
    using Mages.Core.Vm;
    using System;

    public struct Pointer : IMagesType
    {
        public IMemory Memory;
        public Int32 Address;

        public TypeId Type
        {
            get { return TypeId.Pointer; }
        }

        public IMagesType Reference
        {
            get { return Memory.Load(Address); }
            set { Memory.Store(Address, value); }
        }

        public override String ToString()
        {
            return Reference.ToString();
        }
    }
}
