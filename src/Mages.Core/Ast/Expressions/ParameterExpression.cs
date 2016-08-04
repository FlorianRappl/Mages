namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an expression containing function parameters.
    /// </summary>
    public sealed class ParameterExpression : ComputingExpression, IExpression
    {
        #region Fields

        private readonly IExpression[] _parameters;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new parameter expression.
        /// </summary>
        public ParameterExpression(IExpression[] parameters, TextPosition start, TextPosition end)
            : base(start, end)
        {
            _parameters = parameters;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained expressions.
        /// </summary>
        public IExpression[] Parameters
        {
            get { return _parameters; }
        }

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
        /// Validates the expression with the given context.
        /// </summary>
        /// <param name="context">The validator to report errors to.</param>
        public void Validate(IValidationContext context)
        {
            var containsOptional = false;

            foreach (var parameter in _parameters)
            {
                if (parameter is VariableExpression)
                {
                    if (containsOptional)
                    {
                        var error = new ParseError(ErrorCode.OptionalArgumentRequired, parameter);
                        context.Report(error);
                    }
                }
                else if (parameter is AssignmentExpression)
                {
                    var assignment = (AssignmentExpression)parameter;

                    if (assignment.VariableName == null)
                    {
                        var error = new ParseError(ErrorCode.OptionalArgumentRequired, parameter);
                        context.Report(error);
                    }
                    else
                    {
                        containsOptional = true;
                        parameter.Validate(context);
                    }
                }
                else
                {
                    var error = new ParseError(ErrorCode.IdentifierExpected, parameter);
                    context.Report(error);
                }
            }
        }

        #endregion
    }
}
