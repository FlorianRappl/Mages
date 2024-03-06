namespace Mages.Core.Ast.Statements;

/// <summary>
/// Represents an if statement.
/// </summary>
/// <remarks>
/// Creates a new if statement.
/// </remarks>
public sealed class IfStatement(IExpression condition, IStatement primary, IStatement secondary, TextPosition start) : BaseStatement(start, secondary.End), IStatement
{
    #region Fields

    private readonly IExpression _condition = condition;
    private readonly IStatement _primary = primary;
    private readonly IStatement _secondary = secondary;

    #endregion
    #region ctor

    #endregion

    #region Properties

    /// <summary>
    /// Gets the stored condition.
    /// </summary>
    public IExpression Condition => _condition;

    /// <summary>
    /// Gets the primary statement.
    /// </summary>
    public IStatement Primary => _primary;

    /// <summary>
    /// Gets the secondary statement.
    /// </summary>
    public IStatement Secondary => _secondary;

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
