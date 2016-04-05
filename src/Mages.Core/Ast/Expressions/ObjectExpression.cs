namespace Mages.Core.Ast.Expressions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an object literal.
    /// </summary>
    sealed class ObjectExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IDictionary<String, IExpression> _values;

        #endregion

        #region ctor

        public ObjectExpression(IDictionary<String, IExpression> values, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _values = values;
        }

        #endregion

        #region Properties

        public IDictionary<String, IExpression> Values
        {
            get { return _values; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
            foreach (var row in _values)
            {
                row.Value.Validate(context);
            }
        }

        #endregion
    }
}
