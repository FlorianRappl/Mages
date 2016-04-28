namespace Mages.Core.Ast.Statements
{
    sealed class BlockStatement : BaseStatement, IStatement
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
            foreach (var statement in _statements)
            {
                statement.Accept(visitor);
            }
        }

        #endregion
    }
}
