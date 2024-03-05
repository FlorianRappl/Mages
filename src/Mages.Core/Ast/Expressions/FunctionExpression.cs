namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a function expression.
    /// </summary>
    /// <remarks>
    /// Creates a new function expression.
    /// </remarks>
    public sealed class FunctionExpression(AbstractScope scope, ParameterExpression parameters, IStatement body) : ComputingExpression(parameters.Start, body.End), IExpression
    {
        #region Fields

        private readonly AbstractScope _scope = scope;
        private readonly ParameterExpression _parameters = parameters;
        private readonly IStatement _body = body;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the associated abstract scope.
        /// </summary>
        public AbstractScope Scope => _scope;

        /// <summary>
        /// Gets the defined parameters.
        /// </summary>
        public ParameterExpression Parameters => _parameters;

        /// <summary>
        /// Gets the body to use.
        /// </summary>
        public IStatement Body => _body;

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
}
