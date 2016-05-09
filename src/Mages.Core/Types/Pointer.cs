namespace Mages.Core.Types
{
    using System;
    using System.Collections.Generic;

    public struct Pointer : IMagesType
    {
        public IDictionary<String, IMagesType> Scope;
        public String Name;

        public TypeId Type
        {
            get { return TypeId.Pointer; }
        }

        public IMagesType Reference
        {
            get { return Scope[Name]; }
            set { Scope[Name] = value; }
        }

        public override String ToString()
        {
            return Reference.ToString();
        }
    }
}
