﻿namespace Mages.Core.Ast.Expressions;

using System;

/// <summary>
/// Represents an assignment expression.
/// </summary>
/// <remarks>
/// Creates a new assignment expression.
/// </remarks>
public sealed class AssignmentExpression(IExpression variable, IExpression value) : ComputingExpression(variable.Start, value.End), IExpression
{
    #region Fields

    private readonly IExpression _variable = variable;
    private readonly IExpression _value = value;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the variable (value on the left side).
    /// </summary>
    public IExpression Variable => _variable;

    /// <summary>
    /// Gets the variable name, if any.
    /// </summary>
    public String VariableName 
    {
        get 
        {
            if (Variable is VariableExpression variable)
            {
                return variable.Name;
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the value on the right side.
    /// </summary>
    public IExpression Value => _value;

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
        if (!Variable.IsAssignable)
        {
            var error = new ParseError(ErrorCode.AssignableExpected, Variable);
            context.Report(error);
        }

        if (Value is EmptyExpression)
        {
            var error = new ParseError(ErrorCode.AssignmentValueRequired, Value);
            context.Report(error);
        }
    }

    #endregion
}
