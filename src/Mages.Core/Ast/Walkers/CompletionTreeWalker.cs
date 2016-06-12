namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the walker to get code completion information.
    /// </summary>
    public sealed class CompletionTreeWalker : BaseTreeWalker
    {
        private readonly TextPosition _position;
        private readonly IEnumerable<String> _symbols;
        private readonly List<String> _completion;

        /// <summary>
        /// Creates a new completition tree walker for the given position.
        /// </summary>
        public CompletionTreeWalker(TextPosition position, IEnumerable<String> symbols)
        {
            _position = position;
            _symbols = symbols;
            _completion = new List<String>();
        }

        /// <summary>
        /// Gets the list of autocomplete suggestions.
        /// </summary>
        public IEnumerable<String> Suggestions
        {
            get { return _completion.OrderBy(m => m, StringComparer.Ordinal); }
        }

        /// <summary>
        /// Visits a simple statement - accepts the expression.
        /// </summary>
        public override void Visit(SimpleStatement statement)
        {
            if (statement.Start == _position)
            {
                _completion.AddRange(Keywords.GlobalStatementKeywords);
            }

            base.Visit(statement);
        }

        /// <summary>
        /// Visits an empty expression.
        /// </summary>
        public override void Visit(EmptyExpression expression)
        {
            if (Within(expression))
            {
                _completion.AddRange(Keywords.ExpressionKeywords);
                _completion.AddRange(_symbols);
            }

            base.Visit(expression);
        }

        private Boolean Within(ITextRange range)
        {
            return _position >= range.Start && _position <= range.End;
        }
    }
}
