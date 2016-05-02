namespace Mages.Core.Ast.Expressions
{
    using System;

    /// <summary>
    /// Represents a conditional expression.
    /// </summary>
    public sealed class ConditionalExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression _condition;
        private readonly IExpression _primary;
        private readonly IExpression _secondary;

        #endregion

        #region ctor

        public ConditionalExpression(IExpression condition, IExpression primary, IExpression secondary)
            : base(condition.Start, secondary.End)
        {
            _condition = condition;
            _primary = primary;
            _secondary = secondary;
        }

        #endregion

        #region Properties

        public IExpression Condition 
        {
            get { return _condition; }
        }

        public IExpression Primary 
        {
            get { return _primary; }
        }

        public IExpression Secondary
        {
            get { return _secondary; }
        }

        #endregion

        #region Methods

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            _condition.Validate(context);
            _primary.Validate(context);
            _secondary.Validate(context);
        }

        public Func<Object[], Object> GetFunction()
        {
            return args => (Double)args[0] != 0.0 ? args[1] : args[2];
        }

        #endregion
    }
}
