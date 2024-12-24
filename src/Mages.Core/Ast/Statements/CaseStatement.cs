namespace Mages.Core.Ast.Statements;

/// <summary>
/// Represents an case statement.
/// </summary>
/// <remarks>
/// Creates a new case statement.
/// </remarks>
public sealed class CaseStatement(IExpression condition, IStatement body) : BreakableStatement(body, condition.Start, body.End), IStatement
{
    #region Fields

    private readonly IExpression _condition = condition;

    #endregion
    #region ctor

    #endregion

    #region Properties

    /// <summary>
    /// Gets the stored condition.
    /// </summary>
    public IExpression Condition => _condition;

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

    #endregion
}
