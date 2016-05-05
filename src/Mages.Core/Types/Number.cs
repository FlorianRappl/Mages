namespace Mages.Core.Types
{
    using System;
    using System.Globalization;

    public struct Number : IMagesType
    {
        public TypeId Type
        {
            get { return TypeId.Number; }
        }

        public Double Value;

        public Boolean IsTrue
        {
            get { return Value != 0.0; }
        }

        public override String ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
