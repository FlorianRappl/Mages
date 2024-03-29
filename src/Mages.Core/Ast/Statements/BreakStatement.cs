﻿namespace Mages.Core.Ast.Statements;

using Mages.Core.Ast.Expressions;

/// <summary>
/// Represents a break statement.
/// </summary>
/// <remarks>
/// Creates a new break statement with the given payload.
/// </remarks>
/// <param name="expression">The payload.</param>
/// <param name="start">The start position.</param>
/// <param name="end">The end position.</param>
public sealed class BreakStatement(IExpression expression, TextPosition start, TextPosition end) : BaseStatement(start, end), IStatement
{
    #region Fields

    private readonly IExpression _expression = expression;

    #endregion
    #region ctor

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
        if (!context.IsInLoop)
        {
            var error = new ParseError(ErrorCode.LoopMissing, this);
            context.Report(error);
        }

        if (_expression is EmptyExpression == false)
        {
            var error = new ParseError(ErrorCode.TerminatorExpected, _expression);
            context.Report(error);
        }
    }

    #endregion
}
