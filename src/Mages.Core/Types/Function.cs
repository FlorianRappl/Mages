namespace Mages.Core.Types
{
    using System;

    struct Function : IMagesType
    {
        public TypeId Type
        {
            get { return TypeId.Function; }
        }

        public Func<IMagesType[], IMagesType> Value;

        public IMagesType Invoke(IMagesType[] arguments)
        {
            return Value.Invoke(arguments);
        }

        public override String ToString()
        {
            return "[Function]";
        }
    }
}
