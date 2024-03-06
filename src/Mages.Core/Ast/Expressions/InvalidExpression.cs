namespace Mages.Core.Ast.Expressions;

/// <summary>
/// Represents an invalid expression.
/// </summary>
/// <remarks>
/// Creates a new invalid expression.
/// </remarks>
public sealed class InvalidExpression(ErrorCode error, ITextRange payload) : ComputingExpression(payload.Start, payload.End), IExpression
{
    #region Fields

    private readonly ErrorCode _error = error;
    private readonly ITextRange _payload = payload;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the payload covered by the container.
    /// </summary>
    public ITextRange Payload => _payload;

    /// <summary>
    /// Gets the associated error code.
    /// </summary>
    public ErrorCode Error => _error;

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
        context.Report(new ParseError(_error, _payload));
    }

    #endregion
}
