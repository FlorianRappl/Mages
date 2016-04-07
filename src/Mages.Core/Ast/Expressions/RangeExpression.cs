﻿namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// The range expression.
    /// </summary>
    sealed class RangeExpression : ComputingExpression, IExpression
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

        #endregion
    }
}
