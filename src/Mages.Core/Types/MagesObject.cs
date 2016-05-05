namespace Mages.Core.Types
{
    using System;
    using System.Collections.Generic;

    public struct MagesObject : IMagesType
    {
        public TypeId Type
        {
            get { return TypeId.Object; }
        }

        public Dictionary<String, IMagesType> Value;

        public IMagesType GetProperty(String name)
        {
            var result = default(IMagesType);
            Value.TryGetValue(name, out result);
            return result ?? new Undefined();
        }

        public void SetProperty(String name, IMagesType value)
        {
            Value[name] = value;
        }

        public override String ToString()
        {
            return "[Object]";
        }
    }
}
