namespace Mages.Core.Ast.Expressions;

using System;

/// <summary>
/// Base class for all pre unary expressions.
/// </summary>
/// <remarks>
/// Creates a new pre unary expression.
/// </remarks>
public abstract class PreUnaryExpression(TextPosition start, IExpression value, String op) : ComputingExpression(start, value.End), IExpression
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

    internal sealed class Not(TextPosition start, IExpression value) : PreUnaryExpression(start, value, "~")
    {
    }

    internal sealed class Minus(TextPosition start, IExpression value) : PreUnaryExpression(start, value, "-")
    {
    }

    internal sealed class Plus(TextPosition start, IExpression value) : PreUnaryExpression(start, value, "+")
    {
    }

    internal sealed class Type(TextPosition start, IExpression value) : PreUnaryExpression(start, value, "&")
    {
    }

    internal sealed class Increment(TextPosition start, IExpression value) : PreUnaryExpression(start, value, "++")
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

    internal sealed class Decrement(TextPosition start, IExpression value) : PreUnaryExpression(start, value, "--")
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
