namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Vm;
    using Mages.Core.Vm.Operations;
    using System;
    using System.Collections.Generic;

    public class OperationTreeWalker : ITreeWalker
    {
        private readonly List<IOperation> _operations;

        public OperationTreeWalker(List<IOperation> operations)
        {
            _operations = operations;
        }

        public void Visit(EmptyExpression expression)
        {
        }

        public void Visit(ConstantExpression expression)
        {
            var operation = new LoadOperation(expression.Value);
            _operations.Add(operation);
        }

        public void Visit(ArgumentsExpression expression)
        {
            var arguments = expression.Arguments;

            for (var i = arguments.Length - 1; i >= 0; i--)
            {
                arguments[i].Accept(this);
            }
        }

        public void Visit(AssignmentExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(BinaryExpression expression)
        {
            expression.RValue.Accept(this);
            expression.LValue.Accept(this);

            var arguments = new CollectArgsOperation(2);
            _operations.Add(arguments);

            var function = new LoadOperation(expression.GetFunction());
            _operations.Add(function);

            var call = new CallOperation();
            _operations.Add(call);
        }

        public void Visit(PreUnaryExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(PostUnaryExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(RangeExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(ConditionalExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(CallExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(ObjectExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(PropertyExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(MatrixExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(FunctionExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(InvalidExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdentifierExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(MemberExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(ParameterExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(VariableExpression expression)
        {
            throw new NotImplementedException();
        }
    }
}
