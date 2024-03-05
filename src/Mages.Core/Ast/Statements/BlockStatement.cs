namespace Mages.Core.Ast.Statements
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a block of statements.
    /// </summary>
    /// <remarks>
    /// Creates a new block statement.
    /// </remarks>
    public sealed class BlockStatement(IStatement[] statements, TextPosition start, TextPosition end) : BaseStatement(start, end), IStatement
    {
        #region Fields

        private readonly IStatement[] _statements = statements;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained statements.
        /// </summary>
        public IEnumerable<IStatement> Statements => _statements;

        #endregion

        #region Methods

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public void Validate(IValidationContext context)
        {
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
