namespace Mages.Core.Ast.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a constant (predefined value) expression.
    /// </summary>
    /// <remarks>
    /// Creates a new constant expression for the given value.
    /// </remarks>
    public abstract class ConstantExpression(Object value, TextPosition start, TextPosition end) : ComputingExpression(start, end), IExpression
    {
        #region Fields

        private readonly Object _value = value;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the carried value.
        /// </summary>
        public Object Value => _value;

        #endregion

        #region Methods

        /// <summary>
        /// Accepts the visitor by showing him around.
        /// </summary>
        /// <param name="visitor">The visitor walking the tree.</param>
        public void Accept(ITreeWalker visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Creates a new constant expression carrying the given object.
        /// </summary>
        /// <param name="value">The value to carry.</param>
        /// <param name="range">The range that is covered.</param>
        /// <returns>The constant expression.</returns>
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

        /// <summary>
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public virtual void Validate(IValidationContext context)
        {
        }

        #endregion

        #region Operations

        internal sealed class StringConstant(String value, ITextRange range, IEnumerable<ParseError> errors) : ConstantExpression(value, range.Start, range.End)
        {
            private readonly IEnumerable<ParseError> _errors = errors;

            public override void Validate(IValidationContext context)
            {
                base.Validate(context);

                foreach (var error in _errors)
                {
                    context.Report(error);
                }
            }
        }

        internal sealed class BooleanConstant(Boolean value, ITextRange range) : ConstantExpression(value, range.Start, range.End)
        {
        }

        internal sealed class NumberConstant(Double value, ITextRange range, IEnumerable<ParseError> errors) : ConstantExpression(value, range.Start, range.End)
        {
            private readonly IEnumerable<ParseError> _errors = errors;

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
