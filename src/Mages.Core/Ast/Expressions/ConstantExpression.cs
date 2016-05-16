namespace Mages.Core.Ast.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a constant (predefined value) expression.
    /// </summary>
    public abstract class ConstantExpression : ComputingExpression, IExpression
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

        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        public static ConstantExpression From(Object value, ITextRange range)
        {
            if (value is Boolean)
            {
                return new BooleanConstant((Boolean)value, range);
            }
            else if (value is Double)
            {
                return new NumberConstant((Double)value, range, Enumerable.Empty<ParseError>());
            }
            else if (value is String)
            {
                return new StringConstant((String)value, range, Enumerable.Empty<ParseError>());
            }

            throw new InvalidOperationException();
        }

        public virtual void Validate(IValidationContext context)
        {
        }

        #endregion

        #region Operations

        public class StringConstant : ConstantExpression
        {
            private readonly IEnumerable<ParseError> _errors;

            public StringConstant(String value, ITextRange range, IEnumerable<ParseError> errors)
                : base(value, range.Start, range.End)
            {
                _errors = errors;
            }

            public override void Validate(IValidationContext context)
            {
                base.Validate(context);

                foreach (var error in _errors)
                {
                    context.Report(error);
                }
            }
        }

        public class BooleanConstant : ConstantExpression
        {
            public BooleanConstant(Boolean value, ITextRange range)
                : base(value ? 1.0 : 0.0, range.Start, range.End)
            {
            }
        }

        public class NumberConstant : ConstantExpression
        {
            private readonly IEnumerable<ParseError> _errors;

            public NumberConstant(Double value, ITextRange range, IEnumerable<ParseError> errors)
                : base(value, range.Start, range.End)
            {
                _errors = errors;
            }

            public override void Validate(IValidationContext context)
            {
                base.Validate(context);

                foreach (var error in _errors)
                {
                    context.Report(error);
                }
            }
        }

        #endregion
    }
}
