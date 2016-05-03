namespace Mages.Core.Ast.Expressions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a member expression.
    /// </summary>
    public sealed class MemberExpression : AssignableExpression, IExpression
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

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            _obj.Validate(context);
            _member.Validate(context);
        }

        public Func<Object[], Object> GetFunction()
        {
            return args =>
            {
                var obj = (Dictionary<String, Object>)args[0];
                var key = (String)args[1];
                var result = default(Object);
                obj.TryGetValue(key, out result);
                return result;
            };
        }

        #endregion
    }
}
