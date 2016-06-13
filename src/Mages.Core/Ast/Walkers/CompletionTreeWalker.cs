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
        private readonly List<List<String>> _variables;
        private readonly List<String> _completion;
        private readonly Stack<Boolean> _breakable;

        /// <summary>
        /// Creates a new completition tree walker for the given position.
        /// </summary>
        public CompletionTreeWalker(TextPosition position, IEnumerable<String> symbols)
        {
            _position = position;
            _symbols = symbols;
            _completion = new List<String>();
            _breakable = new Stack<Boolean>();
            _breakable.Push(false);
            _variables = new List<List<String>>();
            _variables.Add(new List<String>());
        }

        /// <summary>
        /// Gets the list of autocomplete suggestions.
        /// </summary>
        public IEnumerable<String> Suggestions
        {
            get { return _completion.OrderBy(m => m, StringComparer.Ordinal); }
        }

        /// <summary>
        /// Visits a block statement - accepts all childs.
        /// </summary>
        public override void Visit(BlockStatement block)
        {
            if (!block.Statements.Any() && Within(block))
            {
                AddStatementKeywords();
                AddExpressionKeywords();
            }
            else
            {
                base.Visit(block);
            }
        }

        /// <summary>
        /// Visits a simple statement - accepts the expression.
        /// </summary>
        public override void Visit(SimpleStatement statement)
        {
            if (statement.Start == _position)
            {
                AddStatementKeywords();
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
                AddExpressionKeywords();
            }
        }

        /// <summary>
        /// Visits a binary expression - accepts the left and right value.
        /// </summary>
        public override void Visit(BinaryExpression expression)
        {
            if (Within(expression))
            {
                AddExpressionKeywords();
            }
        }

        /// <summary>
        /// Visits an assignment expression - accepts the variable and value.
        /// </summary>
        public override void Visit(AssignmentExpression expression)
        {
            var name = expression.VariableName;

            if (name != null)
            {
                var c = _variables.Count - 1;

                if (!_variables[c].Contains(name) && !_variables[0].Contains(name))
                {
                    _variables[0].Add(name);
                }
            }

            base.Visit(expression);
        }

        private void AddStatementKeywords()
        {
            _completion.AddRange(Keywords.GlobalStatementKeywords);

            if (_breakable.Peek())
            {
                _completion.AddRange(Keywords.LoopControlKeywords);
            }
        }

        private void AddExpressionKeywords()
        {
            var symbols = _variables.SelectMany(m => m).Concat(_symbols).Distinct();
            _completion.AddRange(Keywords.ExpressionKeywords);
            _completion.AddRange(symbols);            
        }

        private Boolean Within(ITextRange range)
        {
            return _position >= range.Start && _position <= range.End;
        }
    }
}
