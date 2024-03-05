namespace Mages.Core.Ast.Statements
{
    using Mages.Core.Ast.Expressions;

    /// <summary>
    /// Represents a "var ...;" statement.
    /// </summary>
    /// <remarks>
    /// Creates a new var statement.
    /// </remarks>
    public sealed class VarStatement(IExpression assignment, TextPosition start, TextPosition end) : BaseStatement(start, end), IStatement
    {
        #region Fields

        private readonly IExpression _assignment = assignment;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the associated assignment.
        /// </summary>
        public IExpression Assignment => _assignment;

        #endregion

        #region Methods

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public void Validate(IValidationContext context)
        {
            var assignment = _assignment as AssignmentExpression;

            if (assignment == null)
            {
                //TODO Report invalid construction
            }
            else if (assignment.VariableName == null)
            {
                //TODO Report invalid construction
            }
            else
            {
                //TODO Check against variable name (should be first / only 'var' with that name in the _current_ scope)
            }
        }

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
}
