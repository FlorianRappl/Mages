namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SymbolTreeWalker : ITreeWalker
    {
        private readonly IDictionary<VariableExpression, List<VariableExpression>> _collector;
        private readonly IList<VariableExpression> _missing;
        private Boolean _assigning;

        public SymbolTreeWalker()
            : this(new Dictionary<VariableExpression, List<VariableExpression>>(), new List<VariableExpression>())
        {
        }

        public SymbolTreeWalker(IList<VariableExpression> missing)
            : this(new Dictionary<VariableExpression, List<VariableExpression>>(), missing)
        {
        }

        public SymbolTreeWalker(IDictionary<VariableExpression, List<VariableExpression>> collector, IList<VariableExpression> missing)
        {
            _assigning = false;
            _collector = collector;
            _missing = missing;
        }

        public IEnumerable<VariableExpression> Missing
        {
            get { return _missing; }
        }

        public IEnumerable<VariableExpression> Symbols
        {
            get { return _collector.Keys; }
        }

        public IEnumerable<VariableExpression> FindAllReferences(VariableExpression symbol)
        {
            var references = default(List<VariableExpression>);
            
            if (!_collector.TryGetValue(symbol, out references))
            {
                return Enumerable.Empty<VariableExpression>();
            }

            return references;
        }

        public void Visit(BlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                statement.Accept(this);
            }
        }

        public void Visit(SimpleStatement statement)
        {
            statement.Expression.Accept(this);
        }

        public void Visit(VarStatement statement)
        {
            statement.Assignment.Accept(this);
        }

        public void Visit(EmptyExpression expression)
        {
        }

        public void Visit(ConstantExpression expression)
        {
        }

        public void Visit(ArgumentsExpression expression)
        {
            foreach (var argument in expression.Arguments)
            {
                argument.Accept(this);
            }
        }

        public void Visit(AssignmentExpression expression)
        {
            expression.Value.Accept(this);
            _assigning = true;
            expression.Variable.Accept(this);
            _assigning = false;
        }

        public void Visit(BinaryExpression expression)
        {
            expression.LValue.Accept(this);
            expression.RValue.Accept(this);
        }

        public void Visit(PreUnaryExpression expression)
        {
            expression.Value.Accept(this);
        }

        public void Visit(PostUnaryExpression expression)
        {
            expression.Value.Accept(this);
        }

        public void Visit(RangeExpression expression)
        {
            expression.From.Accept(this);
            expression.Step.Accept(this);
            expression.To.Accept(this);
        }

        public void Visit(ConditionalExpression expression)
        {
            expression.Condition.Accept(this);
            expression.Primary.Accept(this);
            expression.Secondary.Accept(this);
        }

        public void Visit(CallExpression expression)
        {
            expression.Function.Accept(this);
            expression.Arguments.Accept(this);
        }

        public void Visit(ObjectExpression expression)
        {
            foreach (var value in expression.Values)
            {
                value.Accept(this);
            }
        }

        public void Visit(PropertyExpression expression)
        {
            expression.Value.Accept(this);
        }

        public void Visit(MatrixExpression expression)
        {
            foreach (var rows in expression.Values)
            {
                foreach (var value in rows)
                {
                    value.Accept(this);
                }
            }
        }

        public void Visit(FunctionExpression expression)
        {
            var scope = expression.Scope;
            var variables = expression.Parameters.Expressions.OfType<VariableExpression>();

            foreach (var variable in variables)
            {
                var expr = new VariableExpression(variable.Name, scope, variable.Start, variable.End);
                var list = new List<VariableExpression>();
                list.Add(expr);
                _collector.Add(expr, list);
            }

            expression.Body.Accept(this);
        }

        public void Visit(InvalidExpression expression)
        {
        }

        public void Visit(IdentifierExpression expression)
        {
        }

        public void Visit(MemberExpression expression)
        {
            expression.Object.Accept(this);
        }

        public void Visit(ParameterExpression expression)
        {
        }

        public void Visit(VariableExpression expression)
        {
            var list = Find(expression.Name, expression.Scope);

            if (list == null && _assigning)
            {
                list = new List<VariableExpression>();
                list.Add(expression);
                _collector[expression] = list;
            }
            else if (list != null)
            {
                list.Add(expression);
            }
            else
            {
                _missing.Add(expression);
            }
        }

        private List<VariableExpression> Find(String name, AbstractScope scope)
        {
            return Symbols.Where(m => m.Name == name && m.Scope == scope).Select(m => _collector[m]).FirstOrDefault();
        }
    }
}
