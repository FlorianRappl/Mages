namespace Mages.Core.Ast.Statements
{
    using Mages.Core.Ast.Expressions;

    /// <summary>
    /// Represents a "var ...;" statement.
    /// </summary>
    sealed class VarStatement : BaseStatement, IStatement
    {
        #region Fields

        private readonly IExpression _assignment;

        #endregion

        #region ctor

        public VarStatement(IExpression assignment, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _assignment = assignment;
        }

        #endregion

        #region Properties

        public IExpression Assignment
        {
            get { return _assignment; }
        }

        #endregion

        #region Methods

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
                assignment.Validate(context);
            }
        }

        public void Accept(ITreeWalker visitor)
        {
            _assignment.Accept(visitor);
        }

        #endregion
    }
}
