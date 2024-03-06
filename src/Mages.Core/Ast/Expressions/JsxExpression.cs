namespace Mages.Core.Ast.Expressions;

/// <summary>
/// Represents an expression containing a JSX subset.
/// </summary>
/// <remarks>
/// Creates a new JSX expression.
/// </remarks>
public sealed class JsxExpression(AbstractScope scope, IExpression element, IExpression[] props, IExpression[] children) : ComputingExpression(element.Start, element.End), IExpression
{
    #region Fields

    private readonly AbstractScope _scope = scope;
    private readonly IExpression _element = element;
    private readonly IExpression[] _props = props;
    private readonly IExpression[] _children = children;

    #endregion
    #region ctor

    #endregion

    #region Properties

    /// <summary>
    /// Gets the associated abstract scope.
    /// </summary>
    public AbstractScope Scope => _scope;

    /// <summary>
    /// Gets the defined element. In case of a fragment this is null.
    /// </summary>
    public IExpression Element => _element;

    /// <summary>
    /// Gets the props to use.
    /// </summary>
    public IExpression[] Props => _props;

    /// <summary>
    /// Gets the children to use.
    /// </summary>
    public IExpression[] Children => _children;

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
