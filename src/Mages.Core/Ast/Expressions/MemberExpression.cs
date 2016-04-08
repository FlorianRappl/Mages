namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents a member expression.
    /// </summary>
    sealed class MemberExpression : AssignableExpression, IExpression
    {
        #region Fields

        private readonly IExpression _obj;
        private readonly IExpression _member;

        #endregion

        #region ctor

        public MemberExpression(IExpression obj, IExpression member)
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

        public IExpression Member
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
