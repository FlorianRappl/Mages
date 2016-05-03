namespace Mages.Core.Vm
{
    using System;

    /// <summary>
    /// Represent's the memory interface.
    /// </summary>
    public interface IMemory
    {
        Object Load(Int32 address);

        void Store(Int32 address, Object value);

        void Clear();
    }
}
