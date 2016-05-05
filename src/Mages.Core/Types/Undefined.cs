namespace Mages.Core.Types
{
    using System;

    public struct Undefined : IMagesType
    {
        public TypeId Type
        {
            get { return TypeId.Undefined; }
        }

        public override String ToString()
        {
            return String.Empty;
        }
    }
}
