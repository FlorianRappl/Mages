namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// The base class for all binary expressions.
    /// </summary>
    abstract class BinaryExpression : ComputingExpression
    {
        #region Fields

        private readonly IExpression _left;
        private readonly IExpression _right;

        #endregion

        #region ctor

        public BinaryExpression(IExpression left, IExpression right)
            : base(left.Start, right.End)
        {
            _left = left;
            _right = right;
        }

        #endregion

        #region Properties

        public IExpression LValue 
        {
            get { return _left; }
        }

        public IExpression RValue
        {
            get { return _right; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            if (_left is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.LeftOperandRequired, LValue.Start);
                context.Report(error);
            }
            else
            {
                _left.Validate(context);
            }

            if (_right is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.RightOperandRequired, RValue.Start);
                context.Report(error);
            }
            else
            {
                _right.Validate(context);
            }
        }

        #endregion

        #region Operations

        public sealed class And : BinaryExpression
        {
            public And(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Or : BinaryExpression
        {
            public Or(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Equal : BinaryExpression
        {
            public Equal(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class NotEqual : BinaryExpression
        {
            public NotEqual(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Greater : BinaryExpression
        {
            public Greater(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Less : BinaryExpression
        {
            public Less(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class GreaterEqual : BinaryExpression
        {
            public GreaterEqual(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class LessEqual : BinaryExpression
        {
            public LessEqual(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Add : BinaryExpression
        {
            public Add(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Subtract : BinaryExpression
        {
            public Subtract(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Multiply : BinaryExpression
        {
            public Multiply(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class LeftDivide : BinaryExpression
        {
            public LeftDivide(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class RightDivide : BinaryExpression
        {
            public RightDivide(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Power : BinaryExpression
        {
            public Power(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class Modulo : BinaryExpression
        {
            public Modulo(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class DotMultiply : BinaryExpression
        {
            public DotMultiply(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class DotLeftDivide : BinaryExpression
        {
            public DotLeftDivide(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class DotRightDivide : BinaryExpression
        {
            public DotRightDivide(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        public sealed class DotPower : BinaryExpression
        {
            public DotPower(IExpression left, IExpression right)
                : base(left, right)
            {
            }
        }

        #endregion
    }
}
