namespace Mages.Core.Ast.Statements;

/// <summary>
/// Represents a breakable statement.
/// </summary>
/// <remarks>
/// Creates a new breakable statement.
/// </remarks>
/// <param name="body">The body to use.</param>
/// <param name="start">The start position.</param>
/// <param name="end">The end position.</param>
public abstract class BreakableStatement(IStatement body, TextPosition start, TextPosition end) : BaseStatement(start, end)
{
    #region Fields

    private readonly IStatement _body = body;

    #endregion
    #region ctor

    #endregion

    #region Properties

    /// <summary>
    /// Gets the stored body.
    /// </summary>
    public IStatement Body => _body;

    #endregion

    #region Methods

    /// <summary>
    /// Validates the expression with the given context.
    /// </summary>
    /// <param name="context">The validator to report errors to.</param>
    public void Validate(IValidationContext context)
    {
    }

    #endregion
}
