namespace Mages.Core.Ast.Expressions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a property (name-value pair) of an object.
    /// </summary>
    public sealed class PropertyExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression _name;
        private readonly IExpression _value;

        #endregion

        #region ctor

        public PropertyExpression(IExpression name, IExpression value)
            : base(name.Start, value.End)
        {
            _name = name;
            _value = value;
        }

        #endregion

        #region Properties

        public IExpression Name
        {
            get { return _name; }
        }

        public IExpression Value
        {
            get { return _value; }
        }

        #endregion

        #region Methods

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            _name.Validate(context);
            _value.Validate(context);
        }

        public Func<Object[], Object> GetFunction()
        {
            return args =>
            {
                var obj = (Dictionary<String, Object>)args[2];
                var key = (String)args[0];
                obj[key] = args[1];
                return obj;
            };
        }

        #endregion
    }
}
