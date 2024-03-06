namespace Mages.Core.Ast.Expressions;

/// <summary>
/// Represents a conditional expression.
/// </summary>
/// <remarks>
/// Creates a new conditional expression.
/// </remarks>
public sealed class ConditionalExpression(IExpression condition, IExpression primary, IExpression secondary) : ComputingExpression(condition.Start, secondary.End), IExpression
{
    #region Fields

    private readonly IExpression _condition = condition;
    private readonly IExpression _primary = primary;
    private readonly IExpression _secondary = secondary;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the condition.
    /// </summary>
    public IExpression Condition => _condition;

    /// <summary>
    /// Gets the primary selected value.
    /// </summary>
    public IExpression Primary => _primary;

    /// <summary>
    /// Gets the alternative selected value.
    /// </summary>
    public IExpression Secondary => _secondary;

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
    }

    #endregion
}
