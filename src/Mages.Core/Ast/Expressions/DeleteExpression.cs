namespace Mages.Core.Ast.Expressions;

/// <summary>
/// Represents a delete expression.
/// </summary>
/// <remarks>
/// Creates a new delete statement with the given payload.
/// </remarks>
/// <param name="start">The start position.</param>
/// <param name="payload">The payload to transport.</param>
public sealed class DeleteExpression(TextPosition start, IExpression payload) : ComputingExpression(start, payload.End), IExpression
{
    #region Fields

    private readonly IExpression _payload = payload;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the stored payload.
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
        var expression = _payload;
        var member = _payload as MemberExpression;
        var isIdentifier = _payload is VariableExpression;

        if (member != null)
        {
            expression = member.Member;
            isIdentifier = expression is IdentifierExpression;
        }

        if (!isIdentifier)
        {
            var error = new ParseError(ErrorCode.IdentifierExpected, expression);
            context.Report(error);
        }
    }

    #endregion
}
