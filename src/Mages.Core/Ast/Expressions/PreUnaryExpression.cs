namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Base class for all pre unary expressions.
    /// </summary>
    public abstract class PreUnaryExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression _value;
        private readonly String _operator;

        #endregion

        #region ctor

        public PreUnaryExpression(TextPosition start, IExpression value, String op)
            : base(start, value.End)
        {
            _value = value;
            _operator = op;
        }

        #endregion

        #region Properties

        public IExpression Value 
        {
            get { return _value; }
        }

        public String Operator
        {
            get { return _operator; }
        }

        #endregion

        #region Methods

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public virtual void Validate(IValidationContext context)
        {
            if (_value is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.OperandRequired, _value);
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
                : base(start, value, "~")
            {
            }
        }

        public class Minus : PreUnaryExpression
        {
            public Minus(TextPosition start, IExpression value)
                : base(start, value, "-")
            {
            }
        }

        public class Plus : PreUnaryExpression
        {
            public Plus(TextPosition start, IExpression value)
                : base(start, value, "+")
            {
            }
        }

        public class Increment : PreUnaryExpression
        {
            public Increment(TextPosition start, IExpression value)
                : base(start, value, "++")
            {
            }

            public override void Validate(IValidationContext context)
            {
                if (Value is VariableExpression == false)
                {
                    var error = new ParseError(ErrorCode.IncrementOperand, Value);
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
                : base(start, value, "--")
            {
            }

            public override void Validate(IValidationContext context)
            {
                if (Value is VariableExpression == false)
                {
                    var error = new ParseError(ErrorCode.DecrementOperand, Value);
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
