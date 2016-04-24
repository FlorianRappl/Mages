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

        private readonly Object _objValue;

        #endregion

        #region ctor

        public ConstantExpression(Object value, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _objValue = value;
        }

        #endregion

        #region Properties

        public Object Value  
        {
            get { return _objValue; }
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
                return new Bool((Boolean)value, range);
            }
            else if (value is Double)
            {
                return new Number((Double)value, range, Enumerable.Empty<ParseError>());
            }
            else if (value is String)
            {
                return new Text((String)value, range, Enumerable.Empty<ParseError>());
            }

            throw new InvalidOperationException();
        }

        public virtual void Validate(IValidationContext context)
        {
        }

        #endregion

        #region Operations

        public class Text : ConstantExpression
        {
            private readonly String _value;
            private readonly IEnumerable<ParseError> _errors;

            public Text(String value, ITextRange range, IEnumerable<ParseError> errors)
                : base(value, range.Start, range.End)
            {
                _value = value;
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

        public class Bool : ConstantExpression
        {
            private readonly Boolean _value;

            public Bool(Boolean value, ITextRange range)
                : base(value, range.Start, range.End)
            {
                _value = value;
            }
        }

        public class Number : ConstantExpression
        {
            private readonly Double _value;
            private readonly IEnumerable<ParseError> _errors;

            public Number(Double value, ITextRange range, IEnumerable<ParseError> errors)
                : base(value, range.Start, range.End)
            {
                _value = value;
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
