namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents a constant (predefined value) expression.
    /// </summary>
    sealed class ConstantExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly Object _value;

        #endregion

        #region ctor

        public ConstantExpression(Object value, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _value = value;
        }

        #endregion

        #region Properties

        public Object Value 
        {
            get { return _value; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
        }

        #endregion
    }
}
