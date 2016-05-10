namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using System.Collections.Generic;

    public class SymbolTreeWalker : ITreeWalker
    {
        private readonly List<VariableExpression> _collector;

        public SymbolTreeWalker(List<VariableExpression> collector)
        {
            _collector = collector;
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
            _collector.Add(expression);
        }
    }
}
