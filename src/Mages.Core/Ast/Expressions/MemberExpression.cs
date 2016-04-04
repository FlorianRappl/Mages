namespace Mages.Core.Ast.Expressions
{
    sealed class MemberExpression : AssignableExpression, IExpression
    {
        #region Fields

        private readonly IExpression _obj;
        private readonly VariableExpression _member;

        #endregion

        #region ctor

        public MemberExpression(IExpression obj, VariableExpression member)
            : base(obj.Start, member.End)
        {
            _obj = obj;
            _member = member;
        }

        #endregion

        #region Properties

        public IExpression Object 
        {
            get { return _obj; }
        }

        public VariableExpression Member
        {
            get { return _member; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            _obj.Validate(context);
            _member.Validate(context);
        }

        #endregion
    }
}
