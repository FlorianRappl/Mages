namespace Mages.Core.Ast.Expressions
{
    using System;
    using Vm;
    /// <summary>
    /// Base class for all pre unary expressions.
    /// </summary>
    public abstract class PreUnaryExpression : ComputingExpression, IExpression
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

        public abstract Func<Object[], Object> GetFunction();

        #endregion

        #region Operations

        public class Not : PreUnaryExpression
        {
            public Not(TextPosition start, IExpression value)
                : base(start, value)
            {
            }

            public override Func<Object[], Object> GetFunction()
            {
                return args => (Double)args[0] == 0.0 ? 1.0 : 0.0;
            }
        }

        public class Minus : PreUnaryExpression
        {
            public Minus(TextPosition start, IExpression value)
                : base(start, value)
            {
            }

            public override Func<Object[], Object> GetFunction()
            {
                return args => -(Double)args[0];
            }
        }

        public class Plus : PreUnaryExpression
        {
            public Plus(TextPosition start, IExpression value)
                : base(start, value)
            {
            }

            public override Func<Object[], Object> GetFunction()
            {
                return args => (Double)args[0];
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
                    var error = new ParseError(ErrorCode.IncrementOperand, Value);
                    context.Report(error);
                }
                else
                {
                    base.Validate(context);
                }
            }

            public override Func<Object[], Object> GetFunction()
            {
                return args =>
                {
                    var p = (Pointer)args[0];
                    var value = (Double)p.Value;
                    return p.Value = value + 1;
                };
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
                    var error = new ParseError(ErrorCode.DecrementOperand, Value);
                    context.Report(error);
                }
                else
                {
                    base.Validate(context);
                }
            }

            public override Func<Object[], Object> GetFunction()
            {
                return args =>
                {
                    var p = (Pointer)args[0];
                    var value = (Double)p.Value;
                    return p.Value = value - 1;
                };
            }
        }

        #endregion
    }
}
