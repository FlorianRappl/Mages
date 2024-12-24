namespace Mages.Core.Ast.Expressions;

/// <summary>
/// The range expression.
/// </summary>
/// <remarks>
/// Creates a new range expression.
/// </remarks>
public sealed class RangeExpression(IExpression from, IExpression step, IExpression to) : ComputingExpression(from.Start, to.End), IExpression
{
    #region Fields

    private readonly IExpression _from = from;
    private readonly IExpression _step = step;
    private readonly IExpression _to = to;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the from expression.
    /// </summary>
    public IExpression From => _from;

    /// <summary>
    /// Gets the to expression.
    /// </summary>
    public IExpression To => _to;

    /// <summary>
    /// Gets the step expression.
    /// </summary>
    public IExpression Step => _step;

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
        if (_from is EmptyExpression)
        {
            var error = new ParseError(ErrorCode.RangeStartRequired, _from);
            context.Report(error);
        }

        if (_to is EmptyExpression)
        {
            var error = new ParseError(ErrorCode.RangeEndRequired, _to);
            context.Report(error);
        }
    }

    #endregion
}
