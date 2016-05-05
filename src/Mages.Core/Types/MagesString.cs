namespace Mages.Core.Types
{
    using System;
    using System.Globalization;

    public struct MagesString : IMagesType
    {
        public TypeId Type
        {
            get { return TypeId.String; }
        }

        public String Value;

        public override String ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
