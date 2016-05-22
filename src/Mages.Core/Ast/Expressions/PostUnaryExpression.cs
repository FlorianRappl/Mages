namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Base class for all post unary expressions.
    /// </summary>
    public abstract class PostUnaryExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression _value;
        private readonly String _operator;

        #endregion

        #region ctor

        public PostUnaryExpression(IExpression value, TextPosition end, String op)
            : base(value.Start, end)
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

        public sealed class Factorial : PostUnaryExpression
        {
            public Factorial(IExpression expression, TextPosition end)
                : base(expression, end, "!")
            {
            }
        }

        public sealed class Transpose : PostUnaryExpression
        {
            public Transpose(IExpression expression, TextPosition end)
                : base(expression, end, "'")
            {
            }
        }

        public sealed class Increment : PostUnaryExpression
        {
            public Increment(IExpression expression, TextPosition end)
                : base(expression, end, "++")
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

        public sealed class Decrement : PostUnaryExpression
        {
            public Decrement(IExpression expression, TextPosition end)
                : base(expression, end, "--")
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
