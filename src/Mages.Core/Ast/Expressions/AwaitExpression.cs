namespace Mages.Core.Ast.Expressions;

/// <summary>
/// Represents a future wrapper.
/// </summary>
/// <remarks>
/// Creates a new await expression.
/// </remarks>
public sealed class AwaitExpression(TextPosition start, IExpression payload) : ComputingExpression(start, payload.End), IExpression
{
    #region Fields

    private readonly IExpression _payload = payload;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the carried payload to be awaited.
    /// </summary>
    public IExpression Payload => _payload;

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
