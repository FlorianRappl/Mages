namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// The base class for all binary expressions.
    /// </summary>
    /// <remarks>
    /// Creates a new binary expression.
    /// </remarks>
    public abstract class BinaryExpression(IExpression left, IExpression right, String op) : ComputingExpression(left.Start, right.End), IExpression
    {
        #region Fields

        private readonly IExpression _left = left;
        private readonly IExpression _right = right;
        private readonly String _operator = op;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value on the left side.
        /// </summary>
        public IExpression LValue => _left;

        /// <summary>
        /// Gets the value on the right side.
        /// </summary>
        public IExpression RValue => _right;

        /// <summary>
        /// Gets the associated operator string.
        /// </summary>
        public String Operator => _operator;

        #endregion

        #region Methods

        /// <summary>
        /// Accepts the visitor by showing him around.
        /// </summary>
        /// <param name="visitor">The visitor walking the tree.</param>
        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public void Validate(IValidationContext context)
        {
            if (_left is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.LeftOperandRequired, LValue);
                context.Report(error);
            }

            if (_right is EmptyExpression)
            {
                var error = new ParseError(ErrorCode.RightOperandRequired, RValue);
                context.Report(error);
            }
        }

        #endregion

        #region Operations

        internal sealed class Pipe(IExpression left, IExpression right) : BinaryExpression(left, right, "|")
        {
        }

        internal sealed class And(IExpression left, IExpression right) : BinaryExpression(left, right, "&&")
        {
        }

        internal sealed class Or(IExpression left, IExpression right) : BinaryExpression(left, right, "||")
        {
        }

        internal sealed class Equal(IExpression left, IExpression right) : BinaryExpression(left, right, "==")
        {
        }

        internal sealed class NotEqual(IExpression left, IExpression right) : BinaryExpression(left, right, "~=")
        {
        }

        internal sealed class Greater(IExpression left, IExpression right) : BinaryExpression(left, right, ">")
        {
        }

        internal sealed class Less(IExpression left, IExpression right) : BinaryExpression(left, right, "<")
        {
        }

        internal sealed class GreaterEqual(IExpression left, IExpression right) : BinaryExpression(left, right, ">=")
        {
        }

        internal sealed class LessEqual(IExpression left, IExpression right) : BinaryExpression(left, right, "<=")
        {
        }

        internal sealed class Add(IExpression left, IExpression right) : BinaryExpression(left, right, "+")
        {
        }

        internal sealed class Subtract(IExpression left, IExpression right) : BinaryExpression(left, right, "-")
        {
        }

        internal sealed class Multiply(IExpression left, IExpression right) : BinaryExpression(left, right, "*")
        {
        }

        internal sealed class LeftDivide(IExpression left, IExpression right) : BinaryExpression(left, right, "\\")
        {
        }

        internal sealed class RightDivide(IExpression left, IExpression right) : BinaryExpression(left, right, "/")
        {
        }

        internal sealed class Power(IExpression left, IExpression right) : BinaryExpression(left, right, "^")
        {
        }

        internal sealed class Modulo(IExpression left, IExpression right) : BinaryExpression(left, right, "%")
        {
        }

        #endregion
    }
}
