namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Base class for all pre unary expressions.
    /// </summary>
    abstract class PreUnaryExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression _value;

        #endregion

        #region ctor

        public PreUnaryExpression(TextPosition start, IExpression value)
            : base(start, value.End)
        {
            _value = value;
        }

        #endregion

        #region Properties

        public IExpression Value 
        {
            get { return _value; }
        }

        #endregion

        #region Methods

        public virtual void Validate(IValidationContext context)
        {
            if (_value is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.OperandRequired, _value.Start);
                context.Report(error);
            }
            else
            {
                _value.Validate(context);
            }
        }

        #endregion

        #region Operations

        public class Not : PreUnaryExpression
        {
            public Not(TextPosition start, IExpression value)
                : base(start, value)
            {
            }
        }

        public class Minus : PreUnaryExpression
        {
            public Minus(TextPosition start, IExpression value)
                : base(start, value)
            {
            }
        }

        public class Plus : PreUnaryExpression
        {
            public Plus(TextPosition start, IExpression value)
                : base(start, value)
            {
            }
        }

        public class Increment : PreUnaryExpression
        {
            public Increment(TextPosition start, IExpression value)
                : base(start, value)
            {
            }

            public override void Validate(IValidationContext context)
            {
                if (Value is VariableExpression == false)
                {
                    var error = new ParseError(ErrorCode.IncrementOperand, Value.Start);
                    context.Report(error);
                }
                else
                {
                    base.Validate(context);
                }
            }
        }

        public class Decrement : PreUnaryExpression
        {
            public Decrement(TextPosition start, IExpression value)
                : base(start, value)
            {
            }

            public override void Validate(IValidationContext context)
            {
                if (Value is VariableExpression == false)
                {
                    var error = new ParseError(ErrorCode.DecrementOperand, Value.Start);
                    context.Report(error);
                }
                else
                {
                    base.Validate(context);
                }
            }
        }

        #endregion
    }
}
