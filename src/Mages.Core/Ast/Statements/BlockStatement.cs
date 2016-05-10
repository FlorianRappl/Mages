namespace Mages.Core.Ast.Statements
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a block of statements.
    /// </summary>
    public sealed class BlockStatement : BaseStatement, IStatement
    {
        #region Fields

        private readonly IStatement[] _statements;

        #endregion

        #region ctor

        public BlockStatement(IStatement[] statements, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _statements = statements;
        }

        #endregion

        #region Properties

        public IEnumerable<IStatement> Statements
        {
            get { return _statements; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            foreach (var statement in _statements)
            {
                statement.Validate(context);
            }
        }

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        #endregion
    }
}
