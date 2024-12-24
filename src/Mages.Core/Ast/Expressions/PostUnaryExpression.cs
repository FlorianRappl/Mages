namespace Mages.Core.Ast.Expressions;

using System;

/// <summary>
/// Base class for all post unary expressions.
/// </summary>
/// <remarks>
/// Creates a new post unary expression.
/// </remarks>
public abstract class PostUnaryExpression(IExpression value, TextPosition end, String op) : ComputingExpression(value.Start, end), IExpression
{
    #region Fields

    private readonly IExpression _value = value;
    private readonly String _operator = op;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the used value.
    /// </summary>
    public IExpression Value => _value;

    /// <summary>
    /// Gets the operator string.
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
    public virtual void Validate(IValidationContext context)
    {
        if (_value is EmptyExpression)
        {
            var error = new ParseError(ErrorCode.OperandRequired, _value);
            context.Report(error);
        }
    }

    #endregion

    #region Operations

    internal sealed class Factorial(IExpression expression, TextPosition end) : PostUnaryExpression(expression, end, "!")
    {
    }

    internal sealed class Transpose(IExpression expression, TextPosition end) : PostUnaryExpression(expression, end, "'")
    {
    }

    internal sealed class Increment(IExpression expression, TextPosition end) : PostUnaryExpression(expression, end, "++")
    {
        public override void Validate(IValidationContext context)
        {
            if (Value is AssignableExpression == false)
            {
                var error = new ParseError(ErrorCode.IncrementOperand, Value);
                context.Report(error);
            }
        }
    }

    internal sealed class Decrement(IExpression expression, TextPosition end) : PostUnaryExpression(expression, end, "--")
    {
        public override void Validate(IValidationContext context)
        {
            if (Value is AssignableExpression == false)
            {
                var error = new ParseError(ErrorCode.DecrementOperand, Value);
                context.Report(error);
            }
        }
    }

    #endregion
}
