namespace Mages.Core.Types
{
    using System;
    using System.Globalization;

    public struct Matrix : IMagesType
    {
        public TypeId Type
        {
            get { return TypeId.Matrix; }
        }

        public Double[,] Value;

        public void Set(Int32 i, Int32 j, Number number)
        {
            Value[i, j] = number.Value;
        }

        public override String ToString()
        {
            var sb = StringBuilderPool.Pull();
            var rows = Value.GetLength(0);
            var cols = Value.GetLength(1);
            sb.Append('[');

            for (var i = 0; i < rows; i++)
            {
                if (i > 0)
                {
                    sb.Append(';');
                }

                for (var j = 0; j < cols; j++)
                {

                    if (j > 0)
                    {
                        sb.Append(',');
                    }

                    sb.Append(Value[i, j].ToString(CultureInfo.InvariantCulture));
                }
            }

            sb.Append(']');
            return sb.Stringify();
        }
    }
}
