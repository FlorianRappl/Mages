namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;

    public abstract class BaseTreeWalker : ITreeWalker
    {
        public virtual void Visit(VarStatement statement)
        {
            statement.Assignment.Accept(this);
        }

        public virtual void Visit(BlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                statement.Accept(this);
            }
        }

        public virtual void Visit(SimpleStatement statement)
        {
            statement.Expression.Accept(this);
        }

        public virtual void Visit(ReturnStatement statement)
        {
            statement.Expression.Accept(this);
        }

        public virtual void Visit(WhileStatement statement)
        {
            statement.Condition.Accept(this);
            statement.Body.Accept(this);
        }

        public virtual void Visit(IfStatement statement)
        {
            statement.Condition.Accept(this);
            statement.Body.Accept(this);
        }

        public virtual void Visit(ContinueStatement statement)
        {
        }

        public virtual void Visit(BreakStatement statement)
        {
        }

        public virtual void Visit(EmptyExpression expression)
        {
        }

        public virtual void Visit(ConstantExpression expression)
        {
        }

        public virtual void Visit(ArgumentsExpression expression)
        {
            foreach (var argument in expression.Arguments)
            {
                argument.Accept(this);
            }
        }

        public virtual void Visit(AssignmentExpression expression)
        {
            expression.Variable.Accept(this);
            expression.Value.Accept(this);
        }

        public virtual void Visit(BinaryExpression expression)
        {
            expression.LValue.Accept(this);
            expression.RValue.Accept(this);
        }

        public virtual void Visit(PreUnaryExpression expression)
        {
            expression.Value.Accept(this);
        }

        public virtual void Visit(PostUnaryExpression expression)
        {
            expression.Value.Accept(this);
        }

        public virtual void Visit(RangeExpression expression)
        {
            expression.From.Accept(this);
            expression.Step.Accept(this);
            expression.To.Accept(this);
        }

        public virtual void Visit(ConditionalExpression expression)
        {
            expression.Condition.Accept(this);
            expression.Primary.Accept(this);
            expression.Secondary.Accept(this);
        }

        public virtual void Visit(CallExpression expression)
        {
            expression.Function.Accept(this);
            expression.Arguments.Accept(this);
        }

        public virtual void Visit(ObjectExpression expression)
        {
            foreach (var value in expression.Values)
            {
                value.Accept(this);
            }
        }

        public virtual void Visit(PropertyExpression expression)
        {
            expression.Name.Accept(this);
            expression.Value.Accept(this);
        }

        public virtual void Visit(MatrixExpression expression)
        {
            foreach (var row in expression.Values)
            {
                foreach (var value in row)
                {
                    value.Accept(this);
                }
            }
        }

        public virtual void Visit(FunctionExpression expression)
        {
            expression.Parameters.Accept(this);
            expression.Body.Accept(this);
        }

        public virtual void Visit(InvalidExpression expression)
        {
        }

        public virtual void Visit(IdentifierExpression expression)
        {
        }

        public virtual void Visit(MemberExpression expression)
        {
            expression.Object.Accept(this);
            expression.Member.Accept(this);
        }

        public virtual void Visit(ParameterExpression expression)
        {
            foreach (var parameter in expression.Parameters)
            {
                parameter.Accept(this);
            }
        }

        public virtual void Visit(VariableExpression expression)
        {
        }
    }
}
