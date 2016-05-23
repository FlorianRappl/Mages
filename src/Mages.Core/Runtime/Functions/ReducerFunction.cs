namespace Mages.Core.Runtime.Functions
{
    using System;

    sealed class ReducerFunction : StandardFunction
    {
        private readonly Func<Double, Double, Double> _reducer;

        public ReducerFunction(Func<Double, Double, Double> reducer)
        {
            _reducer = reducer;
        }

        public override Object Invoke(Double value)
        {
            return value;
        }

        public override Object Invoke(Double[,] matrix)
        {
            var rows = matrix.GetRows();
            var cols = matrix.GetColumns();

            if (rows == 1 && cols == 1)
            {
                return matrix[0, 0];
            }
            else if (rows == 1)
            {
                var element = matrix[0, 0];

                for (var i = 1; i < cols; i++)
                {
                    element = _reducer.Invoke(element, matrix[0, i]);
                }

                return element;
            }
            else if (cols == 1)
            {
                var element = matrix[0, 0];

                for (var i = 1; i < rows; i++)
                {
                    element = _reducer.Invoke(element, matrix[i, 0]);
                }

                return element;
            }
            else
            {
                var result = new Double[rows, 1];

                for (var i = 0; i < rows; i++)
                {
                    var element = matrix[i, 0];

                    for (var j = 0; j < cols; j++)
                    {
                        element = _reducer.Invoke(element, matrix[i, j]);
                    }

                    result[i, 0] = element;
                }

                return result;
            }
        }
    }
}
