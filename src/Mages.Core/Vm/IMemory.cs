namespace Mages.Core.Vm
{
    using Mages.Core.Types;
    using System;

    /// <summary>
    /// Represent's the memory interface.
    /// </summary>
    public interface IMemory
    {
        IMagesType Load(Int32 address);

        void Store(Int32 address, IMagesType value);

        void Clear();
    }
}
