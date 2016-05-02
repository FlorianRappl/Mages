namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// The class for an argument expression.
    /// </summary>
    public sealed class ArgumentsExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression[] _arguments;

        #endregion

        #region ctor

        public ArgumentsExpression(IExpression[] arguments, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _arguments = arguments;
        }

        #endregion

        #region Properties

        public IExpression[] Arguments
        {
            get { return _arguments; }
        }

        public Int32 Count 
        {
            get { return _arguments.Length; }
        }

        #endregion

        #region Methods

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            _arguments.Validate(context);
        }

        #endregion
    }
}
