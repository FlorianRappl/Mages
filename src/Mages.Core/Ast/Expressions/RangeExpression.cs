namespace Mages.Core.Ast.Expressions
{
    using Mages.Core.Types;
    using System;

    /// <summary>
    /// The range expression.
    /// </summary>
    public sealed class RangeExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression _from;
        private readonly IExpression _step;
        private readonly IExpression _to;

        #endregion

        #region ctor

        public RangeExpression(IExpression from, IExpression step, IExpression to)
            : base(from.Start, to.End)
        {
            _from = from;
            _step = step;
            _to = to;
        }

        #endregion

        #region Properties

        public IExpression From
        {
            get { return _from; }
        }

        public IExpression To
        {
            get { return _to; }
        }

        public IExpression Step
        {
            get { return _step; }
        }

        #endregion

        #region Methods

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            if (_from is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.RangeStartRequired, _from);
                context.Report(error);
            }
            else
            {
                _from.Validate(context);
            }

            if (_step is EmptyExpression == false)
            {
                _step.Validate(context);
            }

            if (_to is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.RangeEndRequired, _to);
                context.Report(error);
            }
            else
            {
                _to.Validate(context);
            }
        }

        public Func<IMagesType[], IMagesType> GetFunction()
        {
            return args => new Matrix { Value = Range(((Number)args[0]).Value, ((Number)args[1]).Value, ((Number)args[2]).Value) };
        }

        private static Double[,] Range(Double from, Double to, Double step)
        {
            var count = (to - from) / step;

            if (count < 0)
            {
                count = 0;
            }
            else
            {
                count = Math.Floor(count);
            }

            var result = new Double[(Int32)count, 1];

            for (int i = 0; i < count; i++)
            {
                result[i, 0] = from;
                from += step;
            }

            return result;
        }

        #endregion
    }
}
