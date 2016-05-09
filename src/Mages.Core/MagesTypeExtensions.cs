namespace Mages.Core
{
    using Mages.Core.Types;
    using System;

    public static class MagesTypeExtensions
    {
        public static Double ToNumber(this IMagesType value)
        {
            switch (value.Type)
            {
                case TypeId.Pointer:
                    return ((Pointer)value).Reference.ToNumber();

                case TypeId.Number:
                    return ((Number)value).Value;

                default:
                    throw new InvalidCastException("The given value cannot be casted to a number.");
            }
        }
    }
}
