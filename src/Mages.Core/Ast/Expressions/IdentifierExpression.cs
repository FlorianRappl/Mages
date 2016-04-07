namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents a generalized identifier, which is not a variable.
    /// </summary>
    sealed class IdentifierExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly String _name;

        #endregion

        #region ctor

        public IdentifierExpression(String name, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _name = name;
        }

        #endregion

        #region Properties

        public String Name
        {
            get { return _name; }
        }

        #endregion

        #region Methods

        public void Validate(IValidationContext context)
        {
        }

        #endregion
    }
}
