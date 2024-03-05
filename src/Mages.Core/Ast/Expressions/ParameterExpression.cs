namespace Mages.Core.Ast.Expressions
{
    /// <summary>
    /// Represents an expression containing function parameters.
    /// </summary>
    /// <remarks>
    /// Creates a new parameter expression.
    /// </remarks>
    public sealed class ParameterExpression(IExpression[] parameters, TextPosition start, TextPosition end) : ComputingExpression(start, end), IExpression
    {
        #region Fields

        private readonly IExpression[] _parameters = parameters;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained expressions.
        /// </summary>
        public IExpression[] Parameters => _parameters;

        /// <summary>
        /// Gets the available parameter names.
        /// </summary>
        public ParameterDefinition[] Names
        {
            get
            {
                var names = new ParameterDefinition[_parameters.Length];

                for (var i = 0; i < _parameters.Length; i++)
                {
                    var identifier = _parameters[i] as VariableExpression;
                    var assignment = _parameters[i] as AssignmentExpression;
                    var required = assignment == null;

                    if (!required)
                    {
                        identifier = assignment.Variable as VariableExpression;
                    }

                    if (identifier != null)
                    {
                        names[i] = new ParameterDefinition(identifier.Name, required);
                    }
                }

                return names;
            }
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
